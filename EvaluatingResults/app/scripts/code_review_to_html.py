#!/usr/bin/env python3
"""Render the output of a Copilot code-review run as one static HTML report.

The raw output may contain prose; the findings are the last JSON array in it:
[{"severity": "error|warning", "file": "", "line": 0, "message": ""}, ...]

Usage: code_review_to_html.py <raw-output-file> <output-dir>
"""
import html
import json
import sys
from pathlib import Path


def parse_findings(text):
    decoder = json.JSONDecoder()
    findings = None
    for start, char in enumerate(text):
        if char != "[":
            continue
        try:
            value, _ = decoder.raw_decode(text, start)
        except ValueError:
            continue
        if isinstance(value, list) and all(isinstance(item, dict) for item in value):
            findings = value
    return findings


def render(findings, raw):
    if findings is None:
        summary = "Code review failed: no findings list found in the Copilot output"
        rows = f'<tr><td colspan="4"><pre>{html.escape(raw) or "No output"}</pre></td></tr>'
    else:
        errors = [f for f in findings if str(f.get("severity", "")).lower() == "error"]
        summary = f"{len(errors)} errors · {len(findings) - len(errors)} warnings"
        rows = "\n".join(
            f'<tr class="{"ko" if str(f.get("severity", "")).lower() == "error" else "warn"}">'
            f'<td>{html.escape(str(f.get("severity", "")))}</td>'
            f'<td>{html.escape(str(f.get("file", "")))}</td>'
            f'<td>{html.escape(str(f.get("line", "")))}</td>'
            f'<td>{html.escape(str(f.get("message", "")))}</td></tr>'
            for f in findings
        ) or '<tr><td colspan="4">No issues found</td></tr>'
    return f"""<!doctype html>
<html lang="en">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>Agent code review</title>
<style>
  body {{ font-family: system-ui, sans-serif; max-width: 60rem; margin: 3rem auto; padding: 0 1rem; }}
  table {{ border-collapse: collapse; width: 100%; }}
  th, td {{ text-align: left; padding: .5rem; border-bottom: 1px solid #ddd; vertical-align: top; }}
  tr.warn td:first-child {{ color: #9a6700; font-weight: bold; }}
  tr.ko td:first-child {{ color: #cf222e; font-weight: bold; }}
  pre {{ margin: 0; white-space: pre-wrap; }}
</style>
</head>
<body>
<h1>Agent code review</h1>
<p>{html.escape(summary)}</p>
<table>
<tr><th>Severity</th><th>File</th><th>Line</th><th>Message</th></tr>
{rows}
</table>
</body>
</html>
"""


def main():
    raw_file, out_dir = Path(sys.argv[1]), Path(sys.argv[2])
    raw = raw_file.read_text(encoding="utf-8") if raw_file.exists() else ""
    out_dir.mkdir(parents=True, exist_ok=True)
    (out_dir / "index.html").write_text(render(parse_findings(raw), raw), encoding="utf-8")


if __name__ == "__main__":
    main()
