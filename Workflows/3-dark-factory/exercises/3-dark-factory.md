# Workflows — Exercise 3: Complete a local Dark Factory

## Learning goal

Complete missing control mechanisms in an autonomous development workflow:


You will inspect an incomplete workflow, define its criteria, and close the autonomous task loop.

## Starting state

The application is a small .NET app, `make validate` is green, and the files in `.dark-factory/tasks/` form a queue of pending tasks.

The supplied [AGENTS.md](../app/AGENTS.md) intentionally contains three incomplete phases:

- `Review Phase`
- `Review Decision Controller Gate`;
- `Task Selection Phase`.

The default is`MAX_TASKS=3` in `../app/.dark-factory/config`; completing all ten tasks is an optional extension.


## Part 0 - Initial status

From the `../app` directory, first verify the baseline:

```bash
make factory-preflight
```
if you find any errors in preflight, fix them before proceeding.


## Part 1 — Design the review phase

Complete `Review Phase` and `Review Controller Gate` in `AGENTS.md`.

It must satisfy these acceptance criteria:

The Reviewer:
- A fresh reviewer evaluates the task acceptance criteria, correctness, regressions, architecture, automated checks, error handling, security, maintainability, complexity, and scope.
- The reviewer is read-only: it cannot fix code, commit, or update the run log.
- Findings are classified as `BLOCKING`, `IMPORTANT`, or `SUGGESTION` and include concrete evidence.

The controller:
- Decisions are `GO`, `FIX`, or `STOP`:
  - `GO` : The product is ready to be marked `DONE`.
  - `FIX` : The product is not ready to be marked `DONE`, but a fix round remains.
  - `STOP` : The product is not ready to be marked `DONE`, and no fix round remains. Needs Human intervention.
- `BLOCKING` or `IMPORTANT` findings produce `FIX` while a fix round remains, otherwise `STOP`. Read `MAX_FIX_ROUNDS` from `.dark-factory/config`.
- An unclear product or architecture decision produces `STOP`, not a guessed implementation.
- Route each decision explicitly: `GO` proceeds to ..., `FIX` goes to ..., and `STOP` redirects to ... .

!! Commit your workflow change before continuing and create a checkpoint from a clean working tree:
```bash
git add .
git commit -m "docs(dark-factory): define review and decision gate"
git tag -f dark-factory-review-start
```

In case you want to return to this state, run:
```bash
git reset --hard dark-factory-review-start
```


## Part 2 — Test workflow 'FIX'

Now we're going to test the workflow.
We change the feature definition once it is implemented to see if the review phase is capable of catching the incoherence.
(Script changes Task 001's expected response from `{ "status": "ok" }` to `{ "status": "ready" })

In Copilot paste:

```text
Implement Task 001.
```

!! When the system pauses with `"Workflow testing pause"`,

Run in another terminal:

```bash
make validate # run tests before change feature
make change-feature
make validate
git diff -- .dark-factory/tasks/001-health-endpoint.md
git add .dark-factory/tasks/001-health-endpoint.md
git commit -m "docs(TASK-001): change expected health status"
```

Go back to Copilot and paste:

```text
continue
```

Review should report an evidence-backed required finding: **the implementation fails** the current acceptance criterion even though validation is green. **The controller should choose `FIX`**, repair the implementation and tests, validate, and return control to you again.

At this second pause, write `continue`.

Expect a fresh review, `GO`, and a terminal `DONE` entry.

You finished the TASK-001, CONGRATS!


## Part 3 - Test workflow 'STOP'

Go for TASK-002, do not revert changes. 

Set `MAX_FIX_ROUNDS=0` in `.dark-factory/config`. This allows initial implementation and review, but no repair rounds.

Commit the configuration change and save a checkpoint before Task 002:

```bash
git add .dark-factory/config
git commit -m "test(dark-factory): disable fix rounds for STOP experiment"
git tag -f dark-factory-stop-start
```

In Copilot paste:

```text
Implement Task 002.
```

When the system pauses with `"Workflow testing pause"`, confirm that `make validate` passes. 

In `.dark-factory/tasks/002-required-fields.md`, change the expected response for missing fields from **HTTP 400** to **HTTP 422**. Do not change the implementation or tests.

Run in another terminal:

```bash
make validate
git diff -- .dark-factory/tasks/002-required-fields.md
git add .dark-factory/tasks/002-required-fields.md
git commit -m "docs(TASK-002): require HTTP 422 for missing fields"
```

Go back to Copilot and paste:

```text
continue
```

Review should report that the implementation returns HTTP 400 while the current acceptance criterion requires HTTP 422. Validation remains green, but with `MAX_FIX_ROUNDS=0` the controller must choose **`STOP` directly** and record Task 002 as `STOPPED`. 
It must not repair the implementation, mark the task `DONE`, or start Task 003.


### Look at the results

Record briefly:
- the green validation result before review;
- the review finding and its acceptance-criterion evidence;
- the `STOP` decision, zero fix rounds, and terminal `STOPPED` status;
- why validation alone did not detect the changed requirement.


## Part 4 — Implement the next-task controller

Before Part 4:
- Return to the previous checkpoint:
```bash
git reset --hard dark-factory-stop-start
```
- Remove the manual pause (step 5a) from `AGENTS.md`.
- Restore `MAX_FIX_ROUNDS=2` in `.dark-factory/config`.
- Commit the change.

Now you are ready.

Complete `Task Selection Phase` in `AGENTS.md`: the workflow can now deliver one explicitly named task, but it still cannot operate a queue autonomously. 

Your controller must satisfy these acceptance criteria:
```markdown
    <TODO> 
    CRITERIA TO DECIDE HOW TO SELECT NEXT TASK
    IF NO CRITERIA STOP AND SAY USER: "DEFINE NEXT TASK SELECTION CRITERIA IN `AGENTS.md`"
    </TODO>
```

Open Copilot and paste:

```text
Run the factory.
```

Observe the run from another terminal:

```bash
git log --graph --oneline --decorate --all
tail -n 240 .dark-factory/run-log.md
```

With the default configuration, the run stops after at configured attempted tasks.


## Success criteria

- Task 001 demonstrated that a green validation result is not equivalent to review approval.
- The review policy has an unambiguous `GO`, `FIX`, and `STOP` path.
- Reviewer, fixer, and coordinator responsibilities do not overlap.
- The controller selects tasks deterministically from a durable repository state.
- The factory stops at the configured bound without additional coaching.
- `main` is clean and `make validate` is green after every completed task.
- The Git graph and run log explain what happened without relying on the chat transcript.

## Reflection

- What can an independent reviewer detect that tests cannot?
- Why should the reviewer report findings but not own the completion decision?
- What state must the controller reread after every iteration?
- What failure could cause a naive controller to repeat or skip a task?
- Which decisions still require a human even when validation and review agree?

## Optional extensions

- Increase `MAX_TASKS` to `10` and run the complete queue.
