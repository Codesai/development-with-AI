Run the local Dark Factory end to end without asking for approval when an issue and repository guidance are sufficiently clear.

Start with `make factory-preflight`. Read `AGENTS.md`, `.dark-factory/config`, the issue queue, and the run log. The coordinator is the only role allowed to select issues, change branches, commit, rebase, merge, delete branches, or update the run log. Select the first lexically ordered issue that has no terminal `MERGED` or `STOPPED` entry. Never have more than one active issue.

For every selected issue:

1. Switch to `main`, confirm it is clean, and create `feature/issue-NNN` from it.
2. Spawn a read-only Research subagent. It must inspect the issue, relevant code, architecture, analogous behavior, tests, and validation commands without editing.
3. Spawn a separate read-only Plan subagent using the issue and research result. It must name expected files, intended behavior, tests, implications, and risks without asking for approval.
4. Spawn an Implement subagent with that plan. It makes the smallest application and automated-check changes that meet the acceptance criteria, without Git operations or run-log edits.
5. As coordinator, inspect the diff and create normal implementation commits. Spawn a separate non-editing Validate subagent to run exactly the authoritative `make validate` workflow and report the command, exit code, and diagnostics. It may create ignored build output but must not edit sources or Git state.
6. After validation passes, spawn a fresh read-only Review subagent. It reviews the issue, diff, checks, and architecture with a different mindset and reports `BLOCKING`, `IMPORTANT`, and `SUGGESTION` findings. Scope includes criteria, correctness, regressions, architecture, test quality, errors, security, maintainability, complexity, and scope creep.
7. For validation failures or unresolved `BLOCKING`/`IMPORTANT` findings, spawn a separate Fix subagent to inspect and modify the implementation. As coordinator, inspect its diff and create a normal fix commit. Then use the Validate subagent and a fresh Review subagent again. A fix round is inspect, modify, commit, validate, review. Use at most `MAX_FIX_ROUNDS=2`. Suggestions do not require changes.
8. Rebase the feature branch onto current `main`. Resolve only simple, unambiguous conflicts. Spawn the non-editing Validate subagent to run the complete `make validate` suite again. Ensure the feature branch is clean and all merge prerequisites now pass.
9. Append a complete issue entry above the marker in `.dark-factory/run-log.md` and commit only that evidence on the feature branch. Set `Status: MERGED`, `Review: CLEAN`, `Rebase validation: PASS`, and `Merge: PASS — see Git history`; this prepared entry becomes authoritative atomically when its branch is merged. Record the implementation or fix tip before the log commit rather than attempting to predict the merge SHA.
10. Switch to `main` and merge using `git merge --no-ff -m "Merge issue #NNN: <meaningful summary>" feature/issue-NNN`. Do not edit files on `main`. Optionally delete the merged feature branch.
11. Re-inspect `main` before selecting the next issue; do not carry assumptions forward.

If any stop condition from `AGENTS.md` occurs, append a `Status: STOPPED` entry on the active feature branch when safe, leave all evidence intact, stop immediately, and do not attempt later issues. Do not merge failed work.

Finish when the queue is empty, ten issues have been attempted, or the factory stops. Run `make factory-audit` after a normal completion. Report counts for available, attempted, merged, failed, validation failures, review findings, and fix rounds; the stop condition if any; last successful merge; `main` cleanliness; exact validation/audit results; and `git log --graph --oneline --decorate --all`.
