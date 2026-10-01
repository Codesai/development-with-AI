# Local Dark Factory guidance

## Workflow per-task

1. Task Selection Phase:
    <TODO> 
    Define the criteria how the coordinator selects the next task. 
    Until this phase is completed, work only on a task explicitly named by the user; never guess or continue to another task.
    </TODO>
3. Research phase: Research the task, related code, architecture, analogous behavior, tests, and validation commands. Do not edit.
4. Plan Phase: Plan the smallest change: files, behavior, tests, implications, and risks. Do not edit.
5. Implement Phase: Implement the plan and automated checks.
6. Validate Phase: Inspect the diff, then run `make validate`, if ok commit the implementation. Do not edit.

7. Review Phase: 
    <TODO>
    Define the reviewer contract. 
    IF NO CRITERIA STOP AND SAY USER: "DEFINE REVIEWER CRITERIA IN `AGENTS.md`"
    </TODO>

8. Review Coordinator Gate: 
    <TODO>
    Define the review coordinator rules to decide to GO, FIX, or STOP.
    IF NO CRITERIA STOP AND SAY USER: "DEFINE REVIEWER COORDINATOR CRITERIA IN `AGENTS.md`"
    </TODO>

9. Document Phase: Add an entry using the required shape in `.dark-factory/run-log.md`, above its marker, and commit only that evidence. It must include `Status: MERGED`, `Review: DONE`, `Rebase validation: PASS`, and `Merge: PASS — see Git history`. 
10. Integration Phase: On `main`, merge with `git merge --no-ff -m "Merge task #NNN: <summary>" feature/task-NNN`. Do not edit files on `main`; its only tree change is the merge. Then optionally delete the feature branch.

## Stop and finish

Stop the factory immediately—do not continue to later tasks or merge failed work—when the baseline is unexpectedly red; a requirement needs a meaningful undocumented product or architecture decision; an unsafe or destructive operation is required; or a rebase conflict is not simple and unambiguous. When safe, add a `Status: STOPPED` entry on the active feature branch and preserve all evidence.

Finish when the user-requested task is complete, `MAX_TASKS` tasks have been attempted, or the factory stops. Report available, attempted, merged, failed, validation-failure, review-finding, and fix-round counts; any stop condition.

## Rules

Before starting, read `.dark-factory/config`, `.dark-factory/run-log.md`, this file and the current repository state. `main` is authoritative: never make product changes directly on it.

Run the authoritative `make validate` command. Do not delete, skip, weaken, or bypass tests, checks, assertions, architecture rules, formatting, or compiler errors to obtain a pass.

Use Conventional Commits, for example `feat(TASK-NNN): add health endpoint`, `fix(TASK-NNN): reject empty fields`. 
