#!/usr/bin/env bash
set -euo pipefail

# Installs the no-comments guardrail as a user-level GitHub Copilot CLI
# postToolUse hook: copies the check script into ~/.copilot/hooks/ and writes the
# hook config next to it, so nothing about the guardrail enters the project the
# agent reads. Run once, then start a fresh `copilot` session from
# HarnessingAgents/app so the hook loads.
#
#   ./install-no-comments-hook.sh            install (default)
#   ./install-no-comments-hook.sh uninstall  remove it

harness_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
hooks_dir="$HOME/.copilot/hooks"
script_src="$harness_dir/guardrails/check-no-comments.sh"
script_dst="$hooks_dir/check-no-comments.sh"
config_dst="$hooks_dir/no-comments.json"

fail() { printf 'install-no-comments-hook: %s\n' "$*" >&2; exit 1; }

action="${1:-install}"

if [ "$action" = "uninstall" ]; then
  rm -f "$script_dst" "$config_dst"
  printf 'Removed:\n  %s\n  %s\nStart a fresh copilot session for the change to take effect.\n' \
    "$script_dst" "$config_dst"
  exit 0
fi

[ "$action" = "install" ] || fail "usage: $(basename "$0") [install|uninstall]"
[ -f "$script_src" ] || fail "guardrail script not found at $script_src"

mkdir -p "$hooks_dir"
cp "$script_src" "$script_dst"
chmod +x "$script_dst"

cat > "$config_dst" <<'JSON'
{
  "version": 1,
  "hooks": {
    "postToolUse": [
      {
        "type": "command",
        "matcher": "edit|create|apply_patch",
        "bash": "~/.copilot/hooks/check-no-comments.sh",
        "timeoutSec": 30
      }
    ]
  }
}
JSON

printf 'Installed:\n  %s\n  %s\n\nStart a fresh copilot session from HarnessingAgents/app so the hook loads.\nRemove it later with: %s uninstall\n' \
  "$script_dst" "$config_dst" "$0"
