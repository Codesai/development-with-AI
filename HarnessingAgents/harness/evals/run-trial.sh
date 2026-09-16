#!/usr/bin/env bash
set -euo pipefail

# run-trial.sh - one independent, non-interactive Copilot CLI run of the
# exercises 01/02 confirmation-code prompt, shared by the eval harness in
# exercises 04 and 05.
#
# Copies app/ (as it currently stands - including whatever the student has
# done to AGENTS.md) into a fresh throwaway git repo under results/, so a
# grader that reads `git diff` works unmodified, then runs `copilot -p ... -s`
# there non-interactively. Prints only the trial directory's path on stdout;
# the agent's own output goes to <trial-dir>/agent.log.
#
# Trial repos live under results/ (next to this script), not /tmp - the
# agent's changes never touch the real app/ or this repo's own git state,
# they land in their own throwaway repo that happens to sit inside this one.
# results/ is gitignored except for a placeholder, so trial data stays out of
# version control without needing you to remember to clean it up before a
# commit.
#
# This script does not delete the trial directory - the caller grades it and
# decides whether to keep it around for inspection or remove it.
#
# The agent's output streams live to stderr (and into <trial-dir>/agent.log,
# unprefixed) as it runs, so a slow trial shows what the agent is doing
# instead of sitting silent - the caller's own stdout stays clean (just the
# trial dir path) since this goes to fd 2, not fd 1.
#
# Set RUN_TRIAL_LABEL (e.g. "1/3") to prefix every streamed line on the
# terminal with "[1/3] ", so agent chatter is visually distinct from a
# caller's own "trial N/M" / pass-fail lines when trials run in a loop.
# agent.log itself is never prefixed.
#
# Usage: RUN_TRIAL_LABEL=<label> run-trial.sh [--share]
#   --share   also export the session transcript to <trial-dir>/transcript.md

readonly PROMPT='Add a registration confirmation code to this project. Implement the feature end to end: generate the code when a registration is saved, store it in `interests.txt`, return it in the API response, and show it in the frontend confirmation message. The code format is `AAA-YYYYMMDD-NNN-C` (course prefix, UTC date, daily per-course sequence, check character). Make the smallest change that satisfies this - do not refactor or touch unrelated code. Do not build, run, or otherwise validate the app (no build, no server start, no curl, no manual testing) - just make the code change and stop.'

command -v copilot >/dev/null 2>&1 || { printf 'run-trial: copilot is not on PATH\n' >&2; exit 1; }

share=0
[ "${1:-}" = "--share" ] && share=1

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
app_dir="$(cd "$script_dir/../../app" && pwd)"

results_dir="$script_dir/results"
mkdir -p "$results_dir"
trial_dir="$(mktemp -d "$results_dir/trial-XXXXXX")"

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
label="${RUN_TRIAL_LABEL:-}"
prefix() { if [ -n "$label" ]; then sed -u "s#^#[$label] #"; else cat; fi; }

(
  cd "$trial_dir"
  copilot -p "$PROMPT" -s --stream on --allow-all-tools "${share_flag[@]}"
) 2>&1 | tee "$trial_dir/agent.log" | prefix >&2 || true

printf '%s\n' "$trial_dir"
