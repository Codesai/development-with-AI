# Workflows - Exercise 3 — Run a local Dark Factory

## Learning goal

Deliver a queue of small tasks without human intervention while keeping autonomy bounded by isolated Git branches, deterministic validation, independent review, limited repair rounds, explicit stop conditions, and an observable local history.

## Setup

Commit or revert all the work in progress/changes in workspace and execute: 
```bash
git tag dark-factory-start
make factory-preflight
```

## Run the application

From this `../app` directory, start the development service with:

- `make run` to start application.

Remember the application is available at <http://localhost:8080>, and you can view records on the host in [../app/back/interests.txt](../app/back/interests.txt).

**Hot-reload**: When you save a change, the app updates automatically. This is called **hot reload**: you can see your changes without manually restarting the server.

Also:
- `make logs` to view errors.
- `make stop` to stop the app
- `make run` again after changing Docker settings or dependencies.

## Starting state

`make validate` is green. The API accepts and stores an interest registration. The ten files in `.dark-factory/tasks/` describe deliberately small, cumulative changes; none is implemented yet.

The task files are an immutable queue.

Completion state belongs in `.dark-factory/run-log.md` and Git history, not in the task definitions.

## Part 1

Launch `copilot` from the root of the copied repository and paste:

```text
Implement all work in .dark-factory/tasks folder.
```

Do not coach the factory between tasks. It must stop on its own after the queue is empty, ten tasks have been attempted, or a documented stop condition occurs.

Useful commands while observing the run from another terminal are:

```bash
git log --graph --oneline --decorate --all
tail -n 240 .dark-factory/run-log.md
```

After a normal run completes all ten queued tasks, execute:

```bash
make factory-audit
```

The audit checks the clean final state, authoritative validation, exactly ten ordered `--no-ff` merges, feature ancestry, protected harness files, and matching run-log evidence. A stopped run is intentionally diagnosed from its retained feature branch and log instead of passing the completion audit.

### Success and reflection

A normal completion leaves `main` clean and green, with exactly ten ordered merge commits and one visible feature side per task. A bounded stopped run may have fewer merges and deliberately does not pass the completion audit. Each merge must have a matching log entry with validation and review evidence.

**Reflection:** 
- Did separate implementer and reviewer roles find different problems? 
- Which stop conditions protected product or architectural decisions from being guessed?
- Where should a human remain in the loop: defining the task queue, approving a stop, reviewing a risky change, or releasing to production? Why?
- What does the harness make safe or observable (branches, validation, review, audit trail), and what does it not prove about the product?
- Which risks could still pass `make validate` and the final audit—for example, a misunderstood requirement, a security or privacy risk, or a harmful product decision?
- When should the factory stop and ask for a human decision instead of choosing a plausible implementation by itself?
- How do the run log and the `--no-ff` merge history help a human investigate, approve, or roll back a change?
- Would you trust this workflow for every kind of task? Identify the changes that should require stronger human review or a different validation strategy.
