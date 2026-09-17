#!/usr/bin/env bash
set -euo pipefail

factory_root="$(cd "$(dirname "$0")/.." && pwd -P)"
cd "$factory_root"

fail() { echo "FACTORY AUDIT FAILURE: $*" >&2; exit 1; }

git_root="$(git rev-parse --show-toplevel 2>/dev/null)" || fail "not inside a Git repository"
[[ "$(cd "$git_root" && pwd -P)" == "$factory_root" ]] || fail "audit must run in the standalone exercise repository"
[[ "$(git branch --show-current)" == "main" ]] || fail "final branch must be main"
[[ -z "$(git status --porcelain)" ]] || fail "final working tree is not clean"
git rev-parse --verify dark-factory-start^{commit} >/dev/null 2>&1 || fail "missing dark-factory-start tag"

main_commits=()
while IFS= read -r commit; do
  main_commits+=("$commit")
done < <(git rev-list --first-parent --reverse dark-factory-start..main)
[[ ${#main_commits[@]} -eq 10 ]] || fail "complete audit requires exactly 10 issue merges; found ${#main_commits[@]}"

logged_count="$(grep -Ec '^## Issue [0-9]{3}$' .dark-factory/run-log.md || true)"
[[ "$logged_count" -eq 10 ]] || fail "expected exactly 10 issue log entries, found $logged_count"

previous="$(git rev-parse dark-factory-start)"
expected_issue=1
for merge_commit in "${main_commits[@]}"; do
  read -r commit first_parent feature_tip extra < <(git rev-list --parents -n 1 "$merge_commit")
  [[ -n "${feature_tip:-}" && -z "${extra:-}" ]] || fail "$merge_commit is not a two-parent --no-ff merge"
  [[ "$first_parent" == "$previous" ]] || fail "$merge_commit breaks first-parent order"

  subject="$(git show -s --format=%s "$merge_commit")"
  [[ "$subject" =~ ^Merge\ issue\ \#([0-9]{3}):\ .+ ]] || fail "$merge_commit has an invalid merge subject"
  issue_number="${BASH_REMATCH[1]}"
  expected="$(printf '%03d' "$expected_issue")"
  [[ "$issue_number" == "$expected" ]] || fail "expected merged issue $expected, found $issue_number"

  git merge-base --is-ancestor "$first_parent" "$feature_tip" || fail "issue $issue_number was not based on its current main"
  [[ -z "$(git rev-list --merges "$first_parent..$feature_tip")" ]] || fail "issue $issue_number contains a nested merge"

  entry="$(awk -v heading="## Issue $issue_number" '
    $0 == heading { capture=1 }
    capture && $0 ~ /^## Issue / && $0 != heading { exit }
    capture { print }
  ' .dark-factory/run-log.md)"
  [[ -n "$entry" ]] || fail "run log is missing issue $issue_number"
  [[ "$(grep -Fxc "## Issue $issue_number" .dark-factory/run-log.md)" -eq 1 ]] || fail "run log contains duplicate issue $issue_number entries"
  grep -Fxq 'Status: MERGED' <<<"$entry" || fail "issue $issue_number is not logged as MERGED"
  grep -Fxq "Branch: feature/issue-$issue_number" <<<"$entry" || fail "issue $issue_number has an invalid branch entry"
  for field in Research Plan Implementation 'Review findings'; do
    grep -Eq "^$field: .+" <<<"$entry" || fail "issue $issue_number lacks $field evidence"
  done
  grep -Eq '^Validation: PASS' <<<"$entry" || fail "issue $issue_number lacks passing validation"
  grep -Fxq 'Review: CLEAN' <<<"$entry" || fail "issue $issue_number lacks a clean review"
  grep -Eq '^Fix rounds: [0-2]$' <<<"$entry" || fail "issue $issue_number has an invalid fix count"
  grep -Fxq 'Rebase: PASS' <<<"$entry" || fail "issue $issue_number lacks a passing rebase"
  grep -Eq '^Rebase validation: PASS' <<<"$entry" || fail "issue $issue_number lacks post-rebase validation"
  grep -Eq '^Merge: PASS' <<<"$entry" || fail "issue $issue_number lacks merge evidence"

  implementation_commit="$(sed -n 's/^Implementation commit: //p' <<<"$entry")"
  expected_implementation_commit="$(git rev-parse "$feature_tip^")"
  [[ "$implementation_commit" == "$expected_implementation_commit" ]] \
    || fail "issue $issue_number implementation commit does not match the parent of its log commit"
  changed_by_log="$(git diff-tree --no-commit-id --name-only -r "$feature_tip")"
  [[ "$changed_by_log" == '.dark-factory/run-log.md' ]] \
    || fail "issue $issue_number final feature commit must contain only run-log evidence"

  previous="$merge_commit"
  expected_issue=$((expected_issue + 1))
done

git diff --quiet dark-factory-start..main -- .gitignore README.md AGENTS.md Makefile prompts scripts .dark-factory/config .dark-factory/issues \
  || fail "protected factory harness changed during the run"

make validate
[[ -z "$(git status --porcelain)" ]] || fail "audit validation changed the working tree"

echo "dark factory audit: green (${#main_commits[@]} issue merges)"
git log --graph --oneline --decorate --all
