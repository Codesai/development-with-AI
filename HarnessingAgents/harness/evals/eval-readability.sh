#!/usr/bin/env bash
set -euo pipefail

# eval-readability.sh - exercise 05's eval. Same independent trials as
# eval-no-comments.sh, graded by judge-readability.sh's LLM-as-judge instead
# of a script. Trial directories are kept (not deleted) so you can re-judge
# or inspect them afterward - clean them up yourself when done
# (rm -rf results/trial-*).
#
# Usage:
#   eval-readability.sh [N]
#   eval-readability.sh --repeat-judge <trial-dir> [M]
#
# --repeat-judge holds one trial's code fixed and re-runs only the judge M
# times (default 10), to isolate judge-side non-determinism from agent-side
# non-determinism.

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if [ "${1:-}" = "--repeat-judge" ]; then
  dir="${2:?usage: eval-readability.sh --repeat-judge <trial-dir> [M]}"
  m="${3:-10}"
  for i in $(seq 1 "$m"); do
    printf 'judge run %s/%s: ' "$i" "$m"
    "$script_dir/judge-readability.sh" "$dir"
  done
  exit 0
fi

n="${1:-3}"
passed=0
declare -a dirs=()

for i in $(seq 1 "$n"); do
  printf '\n=== Trial %s/%s: agent run (prefixed [%s/%s] below) ===\n' "$i" "$n" "$i" "$n" >&2
  dir="$(RUN_TRIAL_LABEL="$i/$n" "$script_dir/run-trial.sh")"
  dirs+=("$dir")
  verdict="$("$script_dir/judge-readability.sh" "$dir")"
  printf -- '--- Trial %s/%s: %s ---\n' "$i" "$n" "$verdict" >&2
  case "$verdict" in
    READABLE*) passed=$((passed + 1)) ;;
  esac
done

printf '\n%s/%s judged readable\n' "$passed" "$n"
printf '\nTrial directories (kept - re-run judge-readability.sh on any of them, or use --repeat-judge):\n'
printf '  %s\n' "${dirs[@]}"
