#!/usr/bin/env bash
set -euo pipefail

factory_root="$(cd "$(dirname "$0")/.." && pwd -P)"
cd "$factory_root"

fail() { echo "FACTORY SCAFFOLD FAILURE: $*" >&2; exit 1; }

[[ ! -e .github ]] || fail ".github is forbidden in this local exercise"
for required in AGENTS.md README.md Makefile prompts/dark-factory.md .dark-factory/config .dark-factory/run-log.md; do
  [[ -f "$required" ]] || fail "missing $required"
done

issues=()
while IFS= read -r issue; do
  issues+=("$issue")
done < <(find .dark-factory/issues -maxdepth 1 -type f -name '[0-9][0-9][0-9]-*.md' | sort)
[[ ${#issues[@]} -eq 10 ]] || fail "expected exactly 10 issue files, found ${#issues[@]}"

for index in "${!issues[@]}"; do
  expected="$(printf '%03d' "$((index + 1))")"
  issue="${issues[$index]}"
  [[ "$(basename "$issue")" == "$expected-"* ]] || fail "expected issue $expected, found $(basename "$issue")"
  grep -Fxq "# Issue $expected" <(sed -E '1s/ — .*$//' "$issue") || fail "$issue has an invalid title"
  for heading in '## Goal' '## Acceptance criteria' '## Constraints'; do
    grep -Fxq "$heading" "$issue" || fail "$issue is missing $heading"
  done
done

grep -Fxq 'MAX_ISSUES=10' .dark-factory/config || fail "MAX_ISSUES must be 10"
grep -Fxq 'MAX_FIX_ROUNDS=2' .dark-factory/config || fail "MAX_FIX_ROUNDS must be 2"
grep -Fxq 'MAIN_BRANCH=main' .dark-factory/config || fail "main branch is not configured"

for stage in Research Plan Implement Validate Review Fix Rebase Merge; do
  grep -qi "$stage" prompts/dark-factory.md || fail "prompt does not describe $stage"
done
grep -q -- '--no-ff' AGENTS.md || fail "AGENTS.md does not require --no-ff"
grep -q 'MAX_FIX_ROUNDS=2' prompts/dark-factory.md || fail "prompt does not enforce two fix rounds"

for target in validate factory-scaffold factory-preflight factory-audit; do
  grep -Eq "^$target:" Makefile || fail "Makefile is missing $target"
done

for script in scripts/check-factory-scaffold.sh scripts/preflight-factory.sh scripts/audit-factory.sh; do
  bash -n "$script" || fail "$script has invalid shell syntax"
done

echo "dark factory scaffold: green"
