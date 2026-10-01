# Dark Factory run log

The coordinator appends one section per attempted task. Task definitions remain immutable.

Required entry shape:

```text
## Task NNN

Status: DONE | STOPPED
Research: <relevant files and conventions>
Plan: <intended change and risks>
Implementation: <smallest delivered change>
Implementation commit: <implementation or fix SHA on main before log evidence>
Validation: PASS | FAIL — make validate (exit N)
Review: CLEAN | BLOCKING | IMPORTANT
Review findings: <counts and concise details>
Fix rounds: 0 | 1 | 2
```

<!-- Append issue entries above this line. -->
