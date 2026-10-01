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

5a. **WORKFLOW TESTING ONLY: manual pause for the exercise.** Follow the instructions in [Workflow testing pause](#workflow-testing-pause).

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

## Workflow testing pause

ONLY FOR TESTING PURPOSES.

These instructions apply only to step 5a during the review experiments in Parts 2 and 3. They do not replace the review contract or decision gate.

- While this step is present, the coordinator must pause after every successful validation and implementation/fix commit, before review, including after each fix round.
- Tell the user: "Workflow testing pause: optionally change the current task requirement as instructed in the exercise (`make change-feature` is for Task 001 only) and commit the requirement change, then write `continue`. To keep the requirement unchanged, just write `continue`."
- End the turn and wait for an explicit `continue`. Never run `make change-feature` yourself or remove this step yourself.
- On `continue`, reread the current task definition and proceed directly to Review Phase. Review the current implementation against the current requirement. Do not undo the user's requirement change or restart implementation before review. The normal decision gate then determines GO, FIX, or STOP.
- The student removes this step after the review experiments, before running the autonomous queue in Part 4. Once removed, validation proceeds directly to review without a manual pause.
