#!/usr/bin/env bash
set -euo pipefail

# The gateway for the HarnessingAgents "Gateways" exercise.
#
# Run this from HarnessingAgents/app instead of `copilot`. It launches Copilot
# CLI with the raw filesystem tools removed:
#   - the native view / glob / grep tools are excluded (the model never sees them)
#   - every shell read / list / search command is denied
#   - the only sanctioned file access is `ws` (on PATH via this script)
#
# The flags are per-session, so this launcher IS the gateway - a plain `copilot`
# has none of it. Anything after the script name is passed straight to copilot.

command -v copilot >/dev/null 2>&1 || { printf 'copilot-gated: copilot is not on PATH\n' >&2; exit 1; }

gateway_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

export PATH="$gateway_dir:$PATH"       # `ws` becomes a bare command
export WS_ROOT="$PWD"                  # ws is confined to the launch directory

exec copilot \
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
  --allow-tool='shell(ws)'   --allow-tool='shell(make)' --allow-tool='shell(docker)' --allow-tool='shell(curl)' \
  "$@"
