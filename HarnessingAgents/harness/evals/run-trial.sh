#!/usr/bin/env bash
set -euo pipefail

# run-trial.sh - one independent, non-interactive Copilot CLI run of the
# exercises 01/02 confirmation-code prompt, shared by the eval harness in
# exercises 04 and 05.
#
# Copies app/ (as it currently stands - including whatever the student has
# done to AGENTS.md) into a fresh throwaway git repo, so a grader that reads
# `git diff` works unmodified, then runs `copilot -p ... -s` there
# non-interactively. Prints only the trial directory's path on stdout; the
# agent's own output goes to <trial-dir>/agent.log.
#
# This script does not delete the trial directory - the caller grades it and
# decides whether to keep it around for inspection or remove it.
#
# Usage: run-trial.sh [--share]
#   --share   also export the session transcript to <trial-dir>/transcript.md

readonly PROMPT='Add a registration confirmation code to this project. Implement the feature end to end: generate the code when a registration is saved, store it in `interests.txt`, return it in the API response, and show it in the frontend confirmation message. The code format is `AAA-YYYYMMDD-NNN-C` (course prefix, UTC date, daily per-course sequence, check character).'

command -v copilot >/dev/null 2>&1 || { printf 'run-trial: copilot is not on PATH\n' >&2; exit 1; }

share=0
[ "${1:-}" = "--share" ] && share=1

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
app_dir="$(cd "$script_dir/../../app" && pwd)"

trial_dir="$(mktemp -d "${TMPDIR:-/tmp}/harnessingagents-eval-XXXXXX")"

cp -a "$app_dir/." "$trial_dir/"
git -C "$trial_dir" init -q
git -C "$trial_dir" -c user.email='eval@localhost' -c user.name='eval' add -A
git -C "$trial_dir" -c user.email='eval@localhost' -c user.name='eval' commit -q -m baseline

share_flag=()
[ "$share" = 1 ] && share_flag=(--share="$trial_dir/transcript.md")

# Non-interactive: the agent runs to completion or Copilot's own limits kick
# in, then this returns. A nonzero exit (e.g. the agent gave up) is not
# treated as a script error - an incomplete trial is still a trial, and the
# grader will simply see whatever diff resulted (possibly none).
(
  cd "$trial_dir"
  copilot -p "$PROMPT" -s --allow-all-tools "${share_flag[@]}"
) >"$trial_dir/agent.log" 2>&1 || true

printf '%s\n' "$trial_dir"
