# Guidance

## Workflow per-task

1. Task Selection Phase:
    <TODO>
    Define the criteria how the task controller selects the next task.
    Until this phase is completed, work only on a task explicitly named by the user; never guess or continue to another task.
    </TODO>

2. Research phase: Research the task, related code, architecture, analogous behavior, tests, and validation commands. Do not edit.
3. Plan Phase: Plan the smallest change: files, behavior, tests, implications, and risks. Do not edit.
4. Implement Phase: Implement the plan and automated checks.
5. Validate Phase: Inspect the diff, then run `make validate`. 
   If ok commit the implementation. Do not edit.

6. Review Phase:
    <TODO>
    Define the reviewer contract.
    IF NO CRITERIA STOP AND SAY USER: "DEFINE REVIEWER CRITERIA IN `AGENTS.md`"
    </TODO>

7. Review Controller Gate:
    <TODO>
    Define the review controller rules to decide to GO, FIX, or STOP.
    Define wich phase should go.
    IF NO CRITERIA STOP AND SAY USER: "DEFINE REVIEWER CONTROLLER CRITERIA IN `AGENTS.md`"
    </TODO>

8. Document Phase: When feature done, add an entry using the required shape in `.dark-factory/run-log.md`, and commit. 


## Stop and finish

Stop the factory immediately—do not continue to later tasks or complete failed work—when the baseline is unexpectedly red; a requirement needs a meaningful undocumented product or architecture decision; an unsafe or destructive operation is required. When safe, add a `Status: STOPPED` entry on `main` and preserve all evidence.

Finish when the user-requested task is complete, `MAX_TASKS` tasks have been attempted, or the factory stops. Report available, attempted, completed, failed, validation-failure, review-finding, and fix-round counts; any stop condition.

## Rules

- Before starting, read `.dark-factory/config`, `.dark-factory/run-log.md`, this file and the current repository state.
- Work directly on `main` using trunk-based development. Start each task from a clean working tree and keep only one task active at a time.
- Run validation with `make validate` command. Do not delete, skip, weaken, or bypass tests, checks, assertions, architecture rules, formatting, or compiler errors to obtain a pass.
- Use Conventional Commits, for example `feat(TASK-NNN): add health endpoint`, `fix(TASK-NNN): reject empty fields`.
