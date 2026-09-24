#!/usr/bin/env bash
set -euo pipefail

# judge-readability.sh - LLM-as-judge grader for exercise 05. Takes one trial
# directory (as produced by run-trial.sh) and asks a *second*, independent
# non-interactive `copilot -p` call to judge whether the diff is genuinely
# readable without comments - not just comment-free, which is all a script
# like check-no-comments.js can tell you. Prints one line on stdout:
# "READABLE" or "NOT-READABLE", a dash, then the judge's one-sentence
# reasoning.
#
# The judge call is run from an empty scratch directory, not the trial
# directory itself, with the diff inlined into the prompt - it has no reason
# to touch any tool, so nothing it could do there matters.
#
# Usage: judge-readability.sh <trial-dir>

command -v copilot >/dev/null 2>&1 || { printf 'judge-readability: copilot is not on PATH\n' >&2; exit 1; }

dir="${1:?usage: judge-readability.sh <trial-dir>}"
[ -d "$dir/.git" ] || { printf 'judge-readability: %s is not a trial directory\n' "$dir" >&2; exit 1; }

# `git diff HEAD` alone misses brand-new files (they're untracked, not
# modified) - stage everything first so the diff includes them too. This
# only touches the throwaway trial repo's index, never the real project.
git -C "$dir" add -A
diff="$(git -C "$dir" diff --cached HEAD)"
if [ -z "$diff" ]; then
  printf 'NOT-READABLE - no changes were made\n'
  exit 0
fi

prompt="$(cat <<EOF
You are grading a code change for readability, not for correctness. The
project's guideline is to write code without explanatory comments and rely on
clear names and small functions instead. Judge whether this diff actually
achieves that: would a reader unfamiliar with the change understand the
confirmation-code format and logic just from the code, with no comments to
lean on?

Answer with exactly one line, nothing else: "READABLE" or "NOT-READABLE",
then a dash, then one sentence of reasoning.

Diff:
$diff
EOF
)"

scratch="$(mktemp -d "${TMPDIR:-/tmp}/harnessingagents-judge-XXXXXX")"
trap 'rm -rf "$scratch"' EXIT

# -s output ends with a trailing blank line, so a bare `tail -n 1` grabs
# that instead of the verdict - drop blank lines first, then take the last
# real one.
(cd "$scratch" && copilot -p "$prompt" -s --allow-all-tools) 2>/dev/null | sed '/^[[:space:]]*$/d' | tail -n 1
