#!/usr/bin/env python3
"""Toggle Task 001's expected health status for the manual review exercise."""
from pathlib import Path
import sys

path = Path(__file__).resolve().parents[1] / ".dark-factory/tasks/001-health-endpoint.md"
content = path.read_text()
ok = '{ "status": "ok" }'
ready = '{ "status": "ready" }'

if content.count(ok) == 1 and ready not in content:
    old, new = ok, ready
elif content.count(ready) == 1 and ok not in content:
    old, new = ready, ok
else:
    sys.exit("Cannot change feature: expected exactly one health response criterion (ok or ready). No files changed.")

path.write_text(content.replace(old, new, 1))
print(f"Task 001 acceptance criterion: {old} -> {new}")
print("Implementation and tests unchanged. Inspect git diff, commit the requirement change, then tell the factory: continue.")
