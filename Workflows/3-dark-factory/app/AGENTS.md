# Local Dark Factory guidance

## Rules

Before starting, read `.dark-factory/config`, `.dark-factory/run-log.md`, this file, the selected task, and the current repository state. `main` is authoritative: never make product changes directly on it.

The coordinator alone changes branches, commits, rebases, merges, deletes branches, and updates `.dark-factory/run-log.md`. Use one `feature/task-NNN` branch at a time, created from the latest clean `main`.

The available roles are read-only researcher, read-only planner, implementer, non-editing validator, fresh read-only reviewer, and fixer. Editors, validators, and Git operations must never overlap.

Run the authoritative `make validate` command. Do not delete, skip, weaken, or bypass tests, checks, assertions, architecture rules, formatting, or compiler errors to obtain a pass. 

Use Conventional Commits, for example `feat(TASK-NNN): add health endpoint`, `fix(TASK-NNN): reject empty fields`. Keep commands, exit results, findings, fixes, implementation commit, rebase validation, and merge result observable in Git and the run log.

## Per-task workflow

1. Task Selection Phase: **TODO — the exercise does not yet define how the coordinator selects the next task.** Until this phase is completed, work only on a task explicitly named by the user; never guess or continue to another task.
2. On clean `main`, create `feature/task-NNN`.
3. Research phase: Research the task, related code, architecture, analogous behavior, tests, and validation commands. Do not edit.
4. Plan Phase: Plan the smallest change: files, behavior, tests, implications, and risks. Do not edit or request approval.
5. Implement Phase: Implement the plan and automated checks. Do not perform Git operations or edit the run log.
6. Validate Phase: Inspect the diff, commit the implementation, then run `make validate` with a non-editing validator. Record its command, exit code, and diagnostics. The validator may create ignored build output, but must not change sources or Git state.
7. Review Phase and Decision Gate: **TODO — the exercise does not yet define the reviewer contract or the coordinator's GO, FIX, or STOP decision.** Stop after validation until this phase is completed.
8. Rebase Phase: Rebase onto current `main`. Resolve only simple, unambiguous conflicts. Validate again and ensure the feature branch is clean.
9. Document Phase: Add an entry using the required shape in `.dark-factory/run-log.md`, above its marker, and commit only that evidence. It must include `Status: MERGED`, `Review: CLEAN`, `Rebase validation: PASS`, and `Merge: PASS — see Git history`. Record the implementation or fix tip before this log-only commit.
10. Integration Phase: On `main`, merge with `git merge --no-ff -m "Merge task #NNN: <summary>" feature/task-NNN`. Do not edit files on `main`; its only tree change is the merge. Then optionally delete the feature branch.

## Stop and finish

Stop the factory immediately—do not continue to later tasks or merge failed work—when the baseline is unexpectedly red; a requirement needs a meaningful undocumented product or architecture decision; an unsafe or destructive operation is required; or a rebase conflict is not simple and unambiguous. When safe, add a `Status: STOPPED` entry on the active feature branch and preserve all evidence.

Finish when the user-requested task is complete, `MAX_TASKS` tasks have been attempted, or the factory stops. Report available, attempted, merged, failed, validation-failure, review-finding, and fix-round counts; any stop condition; last successful merge; `main` cleanliness; the exact validation result; and `git log --graph --oneline --decorate --all`.
