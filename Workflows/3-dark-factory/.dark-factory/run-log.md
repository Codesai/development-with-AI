# Dark Factory run log

The coordinator appends one section per attempted issue. Issue definitions remain immutable.

Required entry shape:

```text
## Issue NNN

Status: MERGED | STOPPED
Branch: feature/issue-NNN
Research: <relevant files and conventions>
Plan: <intended change and risks>
Implementation: <smallest delivered change>
Implementation commit: <feature-tip SHA before log evidence>
Validation: PASS | FAIL — make validate (exit N)
Review: CLEAN | BLOCKING | IMPORTANT
Review findings: <counts and concise details>
Fix rounds: 0 | 1 | 2
Rebase: PASS | FAIL
Rebase validation: PASS | FAIL — make validate (exit N)
Merge: PASS — see Git history | NOT MERGED
```

<!-- Append issue entries above this line. -->
