#!/usr/bin/env bash
set -euo pipefail

# eval-no-comments.sh - exercise 04's eval. Runs the confirmation-code prompt
# N times (default 5) via run-trial.sh, and grades each run deterministically
# by pointing exercise 02's guardrail script directly at the resulting diff -
# no hook, no live session, just the finished result.
#
# Quirk worth knowing: check-no-comments.sh behaves differently depending on
# whether its stdout is a TTY. Piped (as here), it always exits 0 - on a pass
# it prints nothing, on a fail it prints a JSON `additionalContext` blob (the
# shape a postToolUse hook expects). So this grades on stdout CONTENT
# (empty = pass), not on the exit code.
#
# Usage: eval-no-comments.sh [N]

readonly N="${1:-5}"
script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
grader="$script_dir/../guardrails/check-no-comments.sh"

[ -x "$grader" ] || { printf 'eval-no-comments: grader not found or not executable at %s\n' "$grader" >&2; exit 1; }

passed=0
declare -a failed_dirs=()

for i in $(seq 1 "$N"); do
  printf 'trial %s/%s... ' "$i" "$N" >&2
  dir="$("$script_dir/run-trial.sh")"
  output="$("$grader" "$dir" 2>"$dir/grade.err")" || true
  printf '%s' "$output" >"$dir/grade.log"
  if [ -z "$output" ]; then
    printf 'pass\n' >&2
    passed=$((passed + 1))
    rm -rf "$dir"
  else
    printf 'FAIL (kept at %s)\n' "$dir" >&2
    failed_dirs+=("$dir")
  fi
done

printf '\n%s/%s passed\n' "$passed" "$N"
if [ "${#failed_dirs[@]}" -gt 0 ]; then
  printf '\nFailed trials (see <dir>/grade.log for the flagged comments, <dir>/agent.log for the run):\n'
  printf '  %s\n' "${failed_dirs[@]}"
fi
