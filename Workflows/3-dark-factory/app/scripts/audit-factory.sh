#!/usr/bin/env bash
set -euo pipefail

factory_root="$(cd "$(dirname "$0")/.." && pwd -P)"
cd "$factory_root"

fail() { echo "FACTORY AUDIT FAILURE: $*" >&2; exit 1; }

git_root="$(git rev-parse --show-toplevel 2>/dev/null)" || fail "not inside a Git repository"
[[ "$(git branch --show-current)" == "main" ]] || fail "final branch must be main"
[[ -z "$(git status --porcelain)" ]] || fail "final working tree is not clean"
logged_count="$(grep -Ec '^## Task [0-9]{3}$' .dark-factory/run-log.md || true)"
[[ "$logged_count" -eq 10 ]] || fail "expected exactly 10 task log entries, found $logged_count"

for expected_task in {1..10}; do
  task_number="$(printf '%03d' "$expected_task")"
  entry="$(awk -v heading="## Task $task_number" '
    $0 == heading { capture=1 }
    capture && $0 ~ /^## Task / && $0 != heading { exit }
    capture { print }
  ' .dark-factory/run-log.md)"
  [[ -n "$entry" ]] || fail "run log is missing task $task_number"
  [[ "$(grep -Fxc "## Task $task_number" .dark-factory/run-log.md)" -eq 1 ]] || fail "run log contains duplicate task $task_number entries"
  grep -Fxq 'Status: DONE' <<<"$entry" || fail "task $task_number is not logged as DONE"
  for field in Research Plan Implementation 'Review findings'; do
    grep -Eq "^$field: .+" <<<"$entry" || fail "task $task_number lacks $field evidence"
  done
  grep -Eq '^Validation: PASS' <<<"$entry" || fail "task $task_number lacks passing validation"
  grep -Fxq 'Review: CLEAN' <<<"$entry" || fail "task $task_number lacks a clean review"
  grep -Eq '^Fix rounds: [0-2]$' <<<"$entry" || fail "task $task_number has an invalid fix count"

  implementation_commit="$(sed -n 's/^Implementation commit: //p' <<<"$entry")"
  git cat-file -e "$implementation_commit^{commit}" 2>/dev/null \
    || fail "task $task_number has an invalid implementation commit"
  [[ -z "$(git rev-list "$implementation_commit" --not main)" ]] \
    || fail "task $task_number implementation commit is not in main history"
done

make validate
[[ -z "$(git status --porcelain)" ]] || fail "audit validation changed the working tree"

echo "dark factory audit: green (10 completed tasks)"
git log --graph --oneline --decorate --all
