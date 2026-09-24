#!/usr/bin/env bash
set -euo pipefail

# eval-no-comments.sh - exercise 04's eval. Runs the confirmation-code prompt
# N times (default 3) via run-trial.sh, and grades each run deterministically
# by pointing exercise 02's guardrail script directly at the resulting diff -
# no hook, no live session, just the finished result.
#
# Quirk worth knowing: check-no-comments.js behaves differently depending on
# whether its stdout is a TTY. Piped (as here), it always exits 0 - on a pass
# it prints nothing, on a fail it prints a JSON `additionalContext` blob (the
# shape a postToolUse hook expects). So this grades on stdout CONTENT
# (empty = pass), not on the exit code.
#
# Usage: eval-no-comments.sh [N]

readonly N="${1:-3}"
script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
grader="$script_dir/../guardrails/check-no-comments.js"

[ -x "$grader" ] || { printf 'eval-no-comments: grader not found or not executable at %s\n' "$grader" >&2; exit 1; }

# The grader's fail output is a JSON `{"additionalContext": "<message>"}` blob
# (see the quirk note above) - pull the message back out so a failure prints
# the actual flagged comments here, not just a directory to go open.
extract_reason() {
  printf '%s' "$1" | node -e '
    let s = "";
    process.stdin.on("data", d => s += d);
    process.stdin.on("end", () => {
      try { process.stdout.write(JSON.parse(s).additionalContext || s); }
      catch { process.stdout.write(s); }
    });
  '
}

passed=0
declare -a failed_dirs=()

for i in $(seq 1 "$N"); do
  printf '\n=== Trial %s/%s: agent run (prefixed [%s/%s] below) ===\n' "$i" "$N" "$i" "$N" >&2
  dir="$(RUN_TRIAL_LABEL="$i/$N" "$script_dir/run-trial.sh")"
  output="$("$grader" "$dir" 2>"$dir/grade.err")" || true
  printf '%s' "$output" >"$dir/grade.log"
  if [ -z "$output" ]; then
    printf -- '--- Trial %s/%s: PASS ---\n' "$i" "$N" >&2
    passed=$((passed + 1))
    rm -rf "$dir"
  else
    printf -- '--- Trial %s/%s: FAIL (kept at %s) ---\n' "$i" "$N" "$dir" >&2
    extract_reason "$output" | sed 's/^/    /' >&2
    failed_dirs+=("$dir")
  fi
done

printf '\n%s/%s passed\n' "$passed" "$N"
if [ "${#failed_dirs[@]}" -gt 0 ]; then
  printf '\nFailed trials (see <dir>/grade.log for the flagged comments, <dir>/agent.log for the run):\n'
  printf '  %s\n' "${failed_dirs[@]}"
fi
