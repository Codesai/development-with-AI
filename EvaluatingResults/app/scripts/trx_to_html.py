#!/usr/bin/env python3
"""Convert the TRX logs of a test run into one static HTML report.

Usage: trx_to_html.py <trx-dir> <output-dir> <title>
"""
import html
import sys
import xml.etree.ElementTree as ET
from pathlib import Path

NS = {"t": "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}


def read_results(trx_dir):
    results = []
    for trx in sorted(trx_dir.glob("*.trx")):
        for result in ET.parse(trx).getroot().iterfind(".//t:UnitTestResult", NS):
            message = result.find("t:Output/t:ErrorInfo/t:Message", NS)
            results.append({
                "name": result.get("testName"),
                "outcome": result.get("outcome"),
                "duration": result.get("duration", ""),
                "message": message.text.strip() if message is not None and message.text else "",
            })
    return results


def render(results, title):
    failed = [r for r in results if r["outcome"] != "Passed"]
    summary = f"{len(results) - len(failed)} passed · {len(failed)} failed"
    rows = "\n".join(
        f'<tr class="{"ok" if r["outcome"] == "Passed" else "ko"}"><td>{html.escape(r["outcome"])}</td>'
        f'<td>{html.escape(r["name"])}</td><td>{html.escape(r["duration"])}</td>'
        f'<td><pre>{html.escape(r["message"])}</pre></td></tr>'
        for r in results
    ) or '<tr><td colspan="4">No test results found</td></tr>'
    return f"""<!doctype html>
<html lang="en">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>{html.escape(title)}</title>
<style>
  body {{ font-family: system-ui, sans-serif; max-width: 60rem; margin: 3rem auto; padding: 0 1rem; }}
  table {{ border-collapse: collapse; width: 100%; }}
  th, td {{ text-align: left; padding: .5rem; border-bottom: 1px solid #ddd; vertical-align: top; }}
  tr.ok td:first-child {{ color: #1a7f37; font-weight: bold; }}
  tr.ko td:first-child {{ color: #cf222e; font-weight: bold; }}
  pre {{ margin: 0; white-space: pre-wrap; }}
</style>
</head>
<body>
<h1>{html.escape(title)}</h1>
<p>{summary}</p>
<table>
<tr><th>Outcome</th><th>Test</th><th>Duration</th><th>Details</th></tr>
{rows}
</table>
</body>
</html>
"""


def main():
    trx_dir, out_dir, title = Path(sys.argv[1]), Path(sys.argv[2]), sys.argv[3]
    out_dir.mkdir(parents=True, exist_ok=True)
    (out_dir / "index.html").write_text(render(read_results(trx_dir), title), encoding="utf-8")


if __name__ == "__main__":
    main()
