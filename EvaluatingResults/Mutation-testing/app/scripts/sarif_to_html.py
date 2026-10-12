#!/usr/bin/env python3
"""Convert the Roslyn SARIF logs of the static analysis into one static HTML report.

Usage: sarif_to_html.py <sarif-dir> <output-dir>
"""
import html
import json
import sys
from collections import Counter
from pathlib import Path
from urllib.parse import unquote, urlparse

LEVELS = ("error", "warning", "note")


def read_findings(sarif_dir, root):
    findings = []
    for sarif in sorted(sarif_dir.glob("*.sarif")):
        for run in json.loads(sarif.read_text(encoding="utf-8"))["runs"]:
            rules = run["tool"]["driver"].get("rules", [])
            for result in run.get("results", []):
                rule = rules[result["ruleIndex"]] if "ruleIndex" in result else {}
                location = result["locations"][0]["physicalLocation"]
                path = Path(unquote(urlparse(location["artifactLocation"]["uri"]).path))
                findings.append({
                    "level": result["level"],
                    "rule": result["ruleId"],
                    "help": rule.get("helpUri", ""),
                    "category": rule.get("properties", {}).get("category", ""),
                    "message": result["message"]["text"],
                    "file": str(path.relative_to(root)) if path.is_relative_to(root) else str(path),
                    "line": location["region"]["startLine"],
                })
    return sorted(findings, key=lambda f: (LEVELS.index(f["level"]) if f["level"] in LEVELS else 9, f["file"], f["line"]))


def render(findings):
    counts = Counter(f["level"] for f in findings)
    summary = " · ".join(f"{counts[l]} {l}{'s' if counts[l] != 1 else ''}" for l in LEVELS if counts[l]) or "No findings"
    rows = []
    for f in findings:
        rule = html.escape(f["rule"])
        rule_cell = f'<a href="{html.escape(f["help"])}" target="_blank">{rule}</a>' if f["help"] else rule
        rows.append(
            f'<tr class="{html.escape(f["level"])}"><td>{html.escape(f["level"])}</td><td>{rule_cell}</td>'
            f'<td>{html.escape(f["category"])}</td><td>{html.escape(f["message"])}</td>'
            f'<td><code>{html.escape(f["file"])}:{f["line"]}</code></td></tr>')
    body = "\n".join(rows) or '<tr><td colspan="5">Nothing to report.</td></tr>'
    return f"""<!doctype html>
<html lang="en">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>Static analysis report</title>
<style>
  body {{ font-family: system-ui, sans-serif; margin: 2rem; }}
  table {{ border-collapse: collapse; width: 100%; }}
  th, td {{ text-align: left; padding: .5rem .75rem; border-bottom: 1px solid #ddd; vertical-align: top; }}
  th {{ background: #f3f3f3; }}
  tr.error td:first-child {{ color: #b00020; font-weight: 600; }}
  tr.warning td:first-child {{ color: #b26a00; font-weight: 600; }}
  code {{ white-space: nowrap; }}
</style>
</head>
<body>
<h1>Static analysis report</h1>
<p>{html.escape(summary)} — Roslyn analyzers (.NET Analyzers and SonarAnalyzer.CSharp)</p>
<table>
<thead><tr><th>Level</th><th>Rule</th><th>Category</th><th>Message</th><th>Location</th></tr></thead>
<tbody>
{body}
</tbody>
</table>
</body>
</html>
"""


def main():
    sarif_dir, out_dir = Path(sys.argv[1]), Path(sys.argv[2])
    findings = read_findings(sarif_dir, Path.cwd())
    out_dir.mkdir(parents=True, exist_ok=True)
    (out_dir / "index.html").write_text(render(findings), encoding="utf-8")
    print(f"Static analysis: {len(findings)} finding(s) -> {out_dir / 'index.html'}")


if __name__ == "__main__":
    main()
