#!/usr/bin/env bash
set -euo pipefail

factory_root="$(cd "$(dirname "$0")/.." && pwd -P)"
cd "$factory_root"

fail() { echo "FACTORY SCAFFOLD FAILURE: $*" >&2; exit 1; }

[[ ! -e .github ]] || fail ".github is forbidden in this local exercise"
for required in AGENTS.md README.md Makefile .dark-factory/config .dark-factory/run-log.md; do
  [[ -f "$required" ]] || fail "missing $required"
done

tasks=()
while IFS= read -r task; do
  tasks+=("$task")
done < <(find .dark-factory/tasks -maxdepth 1 -type f -name '[0-9][0-9][0-9]-*.md' | sort)
[[ ${#tasks[@]} -eq 10 ]] || fail "expected exactly 10 task files, found ${#tasks[@]}"

for index in "${!tasks[@]}"; do
  expected="$(printf '%03d' "$((index + 1))")"
  task="${tasks[$index]}"
  [[ "$(basename "$task")" == "$expected-"* ]] || fail "expected task $expected, found $(basename "$task")"
  grep -Fxq "# Task $expected" <(sed -E '1s/ — .*$//' "$task") || fail "$task has an invalid title"
  for heading in '## Goal' '## Acceptance criteria' '## Constraints'; do
    grep -Fxq "$heading" "$task" || fail "$task is missing $heading"
  done
done

grep -Eq '^MAX_TASKS=[0-9]+$' .dark-factory/config || fail "MAX_TASKS must be a number"
grep -Eq '^MAX_FIX_ROUNDS=[0-9]+$' .dark-factory/config || fail "MAX_FIX_ROUNDS must be a number"

grep -q -- '--no-ff' AGENTS.md || fail "AGENTS.md does not require --no-ff"

for target in validate factory-scaffold factory-preflight factory-audit; do
  grep -Eq "^$target:" Makefile || fail "Makefile is missing $target"
done

for script in scripts/check-factory-scaffold.sh scripts/preflight-factory.sh scripts/audit-factory.sh; do
  bash -n "$script" || fail "$script has invalid shell syntax"
done

echo "dark factory scaffold: green"
