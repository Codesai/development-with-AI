# Workflows — Exercise 3: Complete a local Dark Factory

## Learning goal

Complete two missing control mechanisms in an autonomous development workflow:

1. an independent review phase with an explicit `GO`, `FIX`, or `STOP` decision;
2. a controller that deterministically selects and executes the next task.

The goal is not merely to ask an agent to implement a list. You will inspect an incomplete workflow, define its safety policy, test that policy with a controlled fault, and finally close the autonomous task loop.

## Starting state

The application is a small .NET API. `make validate` is green and the files in `.dark-factory/tasks/` form a queue of pending tasks.

The supplied [AGENTS.md](../app/AGENTS.md) intentionally contains two incomplete phases:

- `Review Phase and Decision Gate`;
- `Task Selection Phase`.

The factory must not invent either policy. Until you complete task selection, you must name each task explicitly.

The default `MAX_TASKS=3` keeps classroom runs short; completing all ten tasks is an optional extension.

From the `../app` directory, first verify the baseline:

```bash
make factory-preflight
```

Do not tag the repository yet. You will first change and test the workflow itself.

## Part 1 — Design the review phase

Complete `Review Phase and Decision Gate` in `AGENTS.md`.

Your phase must define both the reviewer contract and the coordinator decision. It must satisfy these acceptance criteria:

- A fresh reviewer evaluates the task acceptance criteria, correctness, regressions, architecture, automated checks, error handling, security, maintainability, complexity, and scope.
- The reviewer is read-only: it cannot fix code, commit, change branches, update the run log, or merge.
- Findings are classified as `BLOCKING`, `IMPORTANT`, or `SUGGESTION` and include concrete evidence.
- The coordinator—not the reviewer—owns the decision.
- No `BLOCKING` or `IMPORTANT` finding may reach rebase or integration.
- A required finding produces `FIX` while a fix round remains.
- A separate fixer performs the repair; the same validator and a fresh reviewer run again afterward.
- Suggestions are recorded but do not force a fix.
- When `MAX_FIX_ROUNDS` is exhausted with a required finding unresolved, the decision is `STOP`.
- An unclear product or architecture decision produces `STOP`, not a guessed implementation.

Write the policy so another agent could execute it without asking you to interpret it. A useful way to check it is to make a decision table before translating it into workflow instructions.

Commit your workflow change before continuing.

## Part 2 — Prove that review changes the outcome

First ask the factory to implement only Task 001 and stop immediately after validation:

```text
Implement Task 001 only. Execute research, plan, implementation, and validation, then stop before review. Do not rebase, document, merge, or start another task.
```

Confirm that the feature branch passes `make validate`.

Now perform a controlled review probe. In `.dark-factory/tasks/001-health-endpoint.md`, temporarily change the expected response from:

```json
{ "status": "ok" }
```

to:

```json
{ "status": "ready" }
```

Do not change the implementation or its tests, and do not commit this temporary task edit. Ask for the missing phase explicitly:

```text
Run the Review Phase and Decision Gate for the current Task 001 branch. Use a fresh read-only reviewer. Do not edit anything and do not continue to another phase.
```

The important observation is that validation can still be green while review returns an evidence-backed required finding: the implementation no longer satisfies the current acceptance criterion. The coordinator should choose `FIX`, not `GO`.

Restore the task definition to `{ "status": "ok" }`, run the review again, and confirm that the decision becomes `GO`. Then ask the coordinator to finish Task 001 from rebase through integration, without starting Task 002.

Record briefly:

- the validation result before review;
- the review finding produced by the probe;
- the coordinator decision;
- why validation alone did not detect the changed requirement.

## Part 3 — Implement the next-task controller

The workflow can now deliver one explicitly named task, but it still cannot operate a queue autonomously. Complete `Task Selection Phase` in `AGENTS.md`.

Your controller must satisfy these acceptance criteria:

- Read `MAX_TASKS` from `.dark-factory/config`.
- Discover task files from `.dark-factory/tasks/` and order them lexically.
- Use `.dark-factory/run-log.md` as the completion record; do not edit task definitions to mark progress.
- Select the first task without a terminal `MERGED` or `STOPPED` entry.
- Never have more than one active feature task.
- Re-evaluate the repository and run log after every merge instead of relying on an initial in-memory list.
- Stop the whole run after a task is `STOPPED`; do not silently skip it and continue.
- Finish when the queue is empty, `MAX_TASKS` tasks have been attempted, or a stop condition occurs.
- Never infer success from a branch name alone.
- Report why the loop finished and summarize attempted, merged, stopped, validation-failure, review-finding, and fix-round counts.

Commit the controller change. The factory should now need an outcome-oriented prompt rather than a prompt that tells it how to traverse the queue:

```text
Run the factory.
```

Observe the run from another terminal:

```bash
git log --graph --oneline --decorate --all
tail -n 240 .dark-factory/run-log.md
```

With the default configuration, the run stops after at most three attempted tasks. This short batch is enough to exercise selection, implementation, review, repair, integration, and loop termination.

## Success criteria

- Task 001 demonstrated that a green validation result is not equivalent to review approval.
- The review policy has an unambiguous `GO`, `FIX`, and `STOP` path.
- Reviewer, fixer, and coordinator responsibilities do not overlap.
- The controller selects tasks deterministically from durable repository state.
- The factory stops at the configured bound without additional coaching.
- `main` is clean and `make validate` is green after every successful integration.
- The Git graph and run log explain what happened without relying on the chat transcript.

## Reflection

- What can an independent reviewer detect that tests cannot?
- Why should the reviewer report findings but not own the merge decision?
- What state must the controller reread after every iteration?
- What failure could cause a naive controller to repeat or skip a task?
- Which decisions still require a human even when validation and review agree?
- Which parts of this exercise are workflow guarantees, and which are only claims written in the run log?

## Optional extensions

- Increase `MAX_TASKS` to `10` and run the complete queue.
- Add a machine-readable decision record instead of relying only on Markdown.
- Make the integration branch configurable instead of hard-coding `main`.
- Run `make factory-audit` to inspect the strict ten-task Git choreography. This audit is a curiosity tool, not a completion requirement, and only applies to a complete run.
