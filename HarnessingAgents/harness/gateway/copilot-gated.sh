#!/usr/bin/env bash
set -euo pipefail

# The gateway for the HarnessingAgents "Gateways" exercise.
#
# Run this from HarnessingAgents/app instead of `copilot`. It launches Copilot
# CLI with the raw filesystem tools removed:
#   - the native view / glob / grep tools are excluded (the model never sees them)
#   - every shell read / list / search command is denied
#   - the only sanctioned file access is `files-gateway` (on PATH via this script)
#
# The flags are per-session, so this launcher IS the gateway - a plain `copilot`
# has none of it. Anything after the script name is passed straight to copilot.

command -v copilot >/dev/null 2>&1 || { printf 'copilot-gated: copilot is not on PATH\n' >&2; exit 1; }

gateway_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

export PATH="$gateway_dir:$PATH"                # `files-gateway` becomes a bare command
export FILES_GATEWAY_ROOT="$PWD"                # files-gateway is confined to the launch directory

# Also drop a symlink to it right in the launch directory. AGENTS.md tells the
# agent to reach for `files-gateway` once its usual read tools are denied, and
# this makes that tool something it can actually see sitting next to its work
# instead of something it has to know to expect on PATH. Removed on exit so it
# never lingers into other exercises that share this same app/ directory.
ln -sf "$gateway_dir/files-gateway" "$PWD/files-gateway"
trap 'rm -f "$PWD/files-gateway"' EXIT

# Not `exec`: the EXIT trap above only fires on a normal shell exit, and `exec`
# would replace this process before that happens, leaving the symlink behind.
copilot \
  --allow-all-tools \
  --excluded-tools='view,glob,grep' \
  --deny-tool='shell(cat)'   --deny-tool='shell(less)'  --deny-tool='shell(more)' \
  --deny-tool='shell(head)'  --deny-tool='shell(tail)'  --deny-tool='shell(nl)'  --deny-tool='shell(tac)' \
  --deny-tool='shell(ls)'    --deny-tool='shell(dir)'   --deny-tool='shell(vdir)' \
  --deny-tool='shell(find)'  --deny-tool='shell(fd)'    --deny-tool='shell(tree)' \
  --deny-tool='shell(grep)'  --deny-tool='shell(egrep)' --deny-tool='shell(fgrep)' --deny-tool='shell(rg)' \
  --deny-tool='shell(sed)'   --deny-tool='shell(awk)'   --deny-tool='shell(cut)' \
  --deny-tool='shell(xxd)'   --deny-tool='shell(od)'    --deny-tool='shell(strings)' \
  --deny-tool='shell(dd)'    --deny-tool='shell(cp)'    --deny-tool='shell(install)'  --deny-tool='shell(wget)' \
  --deny-tool='shell(python)' --deny-tool='shell(python3)' --deny-tool='shell(node)'   --deny-tool='shell(nodejs)' \
  --deny-tool='shell(perl)'   --deny-tool='shell(ruby)'    --deny-tool='shell(php)'    --deny-tool='shell(lua)' \
  --deny-tool='shell(git)' \
  --allow-tool='shell(files-gateway)' --allow-tool='shell(make)' --allow-tool='shell(docker)' --allow-tool='shell(curl)' \
  "$@"
