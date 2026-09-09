# Local Dark Factory guidance

Read the selected issue, this file, and the current repository state before changing code. `main` is authoritative and product work must never be performed directly on it.

For each issue, execute research, plan, implement, validate, independent review, fix when required, rebase, validate again, and `git merge --no-ff`. Use one `feature/issue-NNN` branch at a time, based on the latest integrated `main`. The coordinator alone owns issue selection, Git operations, and `.dark-factory/run-log.md`.

Use separate subagents sequentially for the phase roles when subagents are available: read-only researcher, read-only planner, implementer, non-editing validator, fresh read-only reviewer, and a separate fixer when needed. Because they share a working tree, never let editors, validators, or Git operations overlap. The validator may create ignored build output but must not edit sources or Git state. The reviewer classifies findings as `BLOCKING`, `IMPORTANT`, or `SUGGESTION`; only the first two require repair.

Run the authoritative `make validate` command. Never delete, skip, weaken, or bypass tests, checks, assertions, architecture rules, formatting, or compiler errors to obtain a pass. Add automated checks for every issue's acceptance criteria and preserve all earlier checks.

Use at most two fix rounds per issue. Stop the entire factory when validation or required review findings remain after round two, a requirement needs a meaningful undocumented product or architecture decision, an unsafe or destructive operation is required, the baseline is unexpectedly red, or a rebase conflict is not simple and unambiguous. Do not continue to later issues after a stop.

Before integration, commit all implementation and fix changes, rebase onto current `main`, and run `make validate` again. Then commit only the completed log evidence, switch to `main`, and merge with `git merge --no-ff -m "Merge issue #NNN: <summary>"`. The only tree change made while on `main` is the merge itself. Delete the feature branch after successful integration.

Process at most ten issues. Keep the run observable in Git and `.dark-factory/run-log.md`, including commands, exit results, findings, fixes, feature-tip commit, rebase validation, and merge result.
