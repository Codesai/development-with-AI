# Workflows - Exercise 3 — Run a local Dark Factory

## Learning goal

Deliver a queue of small issues without human intervention while keeping autonomy bounded by isolated Git branches, deterministic validation, independent review, limited repair rounds, explicit stop conditions, and an observable local history.

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

`make validate` is green. The API accepts and stores an interest registration. The ten files in `.dark-factory/issues/` describe deliberately small, cumulative changes; none is implemented yet.

The issue files are an immutable queue. 

Completion state belongs in `.dark-factory/run-log.md` and Git history, not in the issue definitions.

## Part 1

Launch `copilot` from the root of the copied repository and paste:

```text
Implement all work in .dark-factory/issues folder.
```

Do not coach the factory between issues. It must stop on its own after the queue is empty, ten issues have been attempted, or a documented stop condition occurs.

Useful commands while observing the run from another terminal are:

```bash
git log --graph --oneline --decorate --all
tail -n 240 .dark-factory/run-log.md
```

After a normal run completes all ten queued issues, execute:

```bash
make factory-audit
```

The audit checks the clean final state, authoritative validation, exactly ten ordered `--no-ff` merges, feature ancestry, protected harness files, and matching run-log evidence. A stopped run is intentionally diagnosed from its retained feature branch and log instead of passing the completion audit.

### Success and reflection

A normal completion leaves `main` clean and green, with exactly ten ordered merge commits and one visible feature side per issue. A bounded stopped run may have fewer merges and deliberately does not pass the completion audit. Each merge must have a matching log entry with validation and review evidence.

Did separate implementer and reviewer roles find different problems? Which stop conditions protected product or architectural decisions from being guessed?

The run log records `Merge: PASS` plus the feature-tip commit. The merge commit's own SHA is reported by Git and the final audit because a commit cannot contain its own hash.


## Part 2

After the README run has completed or stopped, act as coordinator for these hypothetical situations. Use the stop and integration rules in `AGENTS.md`.

1. Validation is green, but an IMPORTANT review finding remains after the second fix round. The implementer suggests merging and fixing it in the next issue.
2. Validation and review passed on the feature branch. Rebase then changed the feature's base commit, and the implementer proposes reusing the earlier validation result.
3. The next issue says “reject duplicate registrations” without defining what counts as a duplicate. The implementer offers to choose a policy automatically.

For each situation, choose **continue**, **revalidate**, or **stop**. Name the exact next action, whether merging is allowed, and the evidence or decision required before progress is possible.

Then draft a three-line stop-log entry for one situation: reason, evidence, and preserved branch/next required decision. Use clearly marked hypothetical references rather than inventing successful command results.

Deliver your decisions and log entry only. Do not restart the factory, modify its immutable queue, or add an eleventh issue. The challenge is to apply the controls when a shortcut looks tempting.
