# Local Dark Factory guidance

## Rules

Before starting, read `.dark-factory/config`, `.dark-factory/run-log.md`, this file, the selected issue, and the current repository state. `main` is authoritative: never make product changes directly on it.

The coordinator alone selects issues, changes branches, commits, rebases, merges, deletes branches, and updates `.dark-factory/run-log.md`. Use one `feature/issue-NNN` branch at a time, created from the latest clean `main`. Select the first lexically ordered issue without a terminal `MERGED` or `STOPPED` log entry.

Use these roles sequentially when subagents are available: read-only researcher, read-only planner, implementer, non-editing validator, fresh read-only reviewer, and fixer. Editors, validators, and Git operations must never overlap. 

Run the authoritative `make validate` command. Do not delete, skip, weaken, or bypass tests, checks, assertions, architecture rules, formatting, or compiler errors to obtain a pass. 

Use conventional commit messages. Keep commands, exit results, findings, fixes, implementation commit, rebase validation, and merge result observable in Git and the run log.

## Per-issue workflow

1. On clean `main`, create `feature/issue-NNN`.
2. Research phase: Research the issue, related code, architecture, analogous behavior, tests, and validation commands. Do not edit.
3. Plan Phase: Plan the smallest change: files, behavior, tests, implications, and risks. Do not edit or request approval.
4. Implement Phase: Implement the plan and automated checks. Do not perform Git operations or edit the run log. 
5. Validate Phase: Inspect the diff, commit the implementation, then run `make validate` with a non-editing validator. Record its command, exit code, and diagnostics. The validator may create ignored build output, but must not change sources or Git state. 
6. Reviewer Phase: Have a fresh reviewer assess acceptance criteria, correctness, regressions, architecture, tests, errors, security, maintainability, complexity, and scope. Classify findings as `BLOCKING`, `IMPORTANT`, or `SUGGESTION`; repair the first two only.
7. Fixing Phase: For a validation failure or unresolved `BLOCKING`/`IMPORTANT` finding, use a separate fixer. Inspect and commit the fix, then validate and review again. A fix round is fix, commit, validate, and review. Use at most `MAX_FIX_ROUNDS` (2); suggestions do not require a fix.
8. Rebase Phase: Rebase onto current `main`. Resolve only simple, unambiguous conflicts. Validate again and ensure the feature branch is clean.
9. Document Phase: Add an entry using the required shape in `.dark-factory/run-log.md`, above its marker, and commit only that evidence. It must include `Status: MERGED`, `Review: CLEAN`, `Rebase validation: PASS`, and `Merge: PASS — see Git history`. Record the implementation or fix tip before this log-only commit.
10. Integration Phase: On `main`, merge with `git merge --no-ff -m "Merge issue #NNN: <summary>" feature/issue-NNN`. Do not edit files on `main`; its only tree change is the merge. Then optionally delete the feature branch.

## Stop and finish

Stop the factory immediately—do not continue to later issues or merge failed work—when the baseline is unexpectedly red; validation or required findings remain after two fix rounds; a requirement needs a meaningful undocumented product or architecture decision; an unsafe or destructive operation is required; or a rebase conflict is not simple and unambiguous. When safe, add a `Status: STOPPED` entry on the active feature branch and preserve all evidence.

Finish when the queue is empty, `MAX_ISSUES` issues have been attempted, or the factory stops. After a normal completion, run `make factory-audit`. Report available, attempted, merged, failed, validation-failure, review-finding, and fix-round counts; any stop condition; last successful merge; `main` cleanliness; exact validation and audit results; and `git log --graph --oneline --decorate --all`.
