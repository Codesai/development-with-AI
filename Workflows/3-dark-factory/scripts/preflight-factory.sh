#!/usr/bin/env bash
set -euo pipefail

factory_root="$(cd "$(dirname "$0")/.." && pwd -P)"
cd "$factory_root"

fail() { echo "FACTORY PREFLIGHT FAILURE: $*" >&2; exit 1; }

git_root="$(git rev-parse --show-toplevel 2>/dev/null)" || fail "not inside a Git repository"
[[ "$(cd "$git_root" && pwd -P)" == "$factory_root" ]] || fail "copy the exercise into its own Git repository"
[[ "$(git branch --show-current)" == "main" ]] || fail "preflight must run on main"
[[ -z "$(git status --porcelain)" ]] || fail "working tree is not clean"
git rev-parse --verify dark-factory-start^{commit} >/dev/null 2>&1 || fail "missing dark-factory-start tag"
[[ "$(git rev-parse dark-factory-start)" == "$(git rev-parse HEAD)" ]] || fail "dark-factory-start must identify the initial main commit"

make factory-scaffold
make validate
[[ -z "$(git status --porcelain)" ]] || fail "validation changed the working tree"

echo "dark factory preflight: green"

