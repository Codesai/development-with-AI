#!/usr/bin/env bash
set -euo pipefail

# Guardrail for the HarnessingAgents "Guardrails" exercise.
#
# Wired into GitHub Copilot CLI's postToolUse hook, this script reports comments
# in any source file the agent has changed. postToolUse runs after the edit is
# already applied, so this cannot block the change; it returns an
# additionalContext payload that the agent sees on its next turn and can act on.
#
# Scope: every source file that differs from HEAD is scanned in full, so both
# newly added and pre-existing comments in a touched file are reported. Reliable
# per-method scoping would need a real parser (`git diff --function-context`
# over-expands to the enclosing class), so the rule is simply: a file you edit
# must contain no comments. It does not parse the source, so a "//" inside a
# string literal is a false positive, and a "*"-prefixed continuation line is
# flagged as a doc-comment line.

readonly MAX_REPORT="${MAX_REPORT:-20}"
readonly DIFF_GLOBS=('*.cs' '*.js' '*.ts' '*.html' '*.css')

fail() { printf 'check-no-comments: %s\n' "$*" >&2; exit 1; }

command -v git >/dev/null 2>&1 || fail "git is required"

target_dir="${1:-$PWD}"
cd "$target_dir" || fail "cannot cd into ${target_dir}"
git rev-parse --show-toplevel >/dev/null 2>&1 || fail "${target_dir} is not a git working tree"

scan_file() {
  awk -v path="$1" '
    {
      raw = $0
      trimmed = raw
      sub(/^[ \t]+/, "", trimmed)
      urlless = raw
      gsub(/:\/\//, "", urlless)
      if (urlless ~ /\/\// || raw ~ /\/\*/ || raw ~ /\*\// || trimmed ~ /^\*/ || raw ~ /<!--/ || raw ~ /-->/) {
        printf "%s:%d: %s\n", path, NR, trimmed
      }
    }
  ' "$1"
}

changed_comments() {
  git diff --name-only --relative HEAD -- "${DIFF_GLOBS[@]}" | while IFS= read -r f; do
    [ -f "$f" ] && scan_file "$f"
  done
}

offenders="$(changed_comments || true)"

if [ -z "$offenders" ]; then
  exit 0
fi

count="$(printf '%s\n' "$offenders" | grep -c . || true)"
shown="$(printf '%s\n' "$offenders" | head -n "$MAX_REPORT" || true)"

message="The no-comments guardrail found ${count} comment(s) in files you just changed:

${shown}

Remove every comment from these files, including ones that were already there before your edit. Rely on descriptive names and small functions instead; do not add explanatory comments to the code."

if [ -t 1 ] || ! command -v node >/dev/null 2>&1; then
  printf '%s\n' "$message" >&2
  exit 2
fi

node -e 'process.stdout.write(JSON.stringify({ additionalContext: process.argv[1] }))' "$message"
exit 0
