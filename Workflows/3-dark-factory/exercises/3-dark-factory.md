# Workflows - Exercise 3 — Run a local Dark Factory

A small API in .NET 10, that receives interest records and saves them to `interests.txt`.

## Requirements

- Use GitHub Codespaces

OR 

- Docker and Docker Compose

## Run the application

From this repository directory, start the development service with:

```bash
make run
```

The application is available at <http://localhost:8080>.

Docker mounts `front/` and `back/` into the development container. Saving HTML or
JavaScript refreshes the browser automatically through `dotnet watch`. Backend
changes are hot-reloaded, or the server restarts automatically when required.
The homepage is `/`; API routes use the same host, including `POST /api/register`.
Use `make logs` to follow build errors and reload events, and `make stop` to stop.
After changing Docker configuration or dependencies, run `make run` again.
Container build outputs use separate volumes so they do not change host file ownership.

The production image remains available with `docker build --target runtime -t interest-app .`.

View records on the host in back/interests.txt (it updates as submissions arrive)

## Learning goal

Deliver a queue of small issues without human intervention while keeping autonomy bounded by isolated Git branches, deterministic validation, independent review, limited repair rounds, explicit stop conditions, and an observable local history.

## Setup

Copy this directory out of the course repository so its issue branches cannot affect the course history. Initialize the copy as its own repository:

```bash
cp -R exercise-08-dark-factory ../dark-factory-lab
cd ../dark-factory-lab
git init -b main
git add .
git commit -m "chore: add dark factory starter"
git tag dark-factory-start
make factory-preflight
```

## Starting state

`make validate` is green. The API accepts and stores an interest registration. The ten files in `.dark-factory/issues/` describe deliberately small, cumulative changes; none is implemented yet.

The issue files are an immutable queue. Completion state belongs in `.dark-factory/run-log.md` and Git history, not in the issue definitions.

## Run

Launch `copilot` from the root of the copied repository and paste `prompts/dark-factory.md`. Do not coach the factory between issues. It must stop on its own after the queue is empty, ten issues have been attempted, or a documented stop condition occurs.

Useful commands while observing the run from another terminal are:

```bash
git log --graph --oneline --decorate --all
sed -n '1,240p' .dark-factory/run-log.md
```

After a normal run completes all ten queued issues, execute:

```bash
make factory-audit
```

The audit checks the clean final state, authoritative validation, exactly ten ordered `--no-ff` merges, feature ancestry, protected harness files, and matching run-log evidence. A stopped run is intentionally diagnosed from its retained feature branch and log instead of passing the completion audit.

## Success and reflection

A normal completion leaves `main` clean and green, with exactly ten ordered merge commits and one visible feature side per issue. A bounded stopped run may have fewer merges and deliberately does not pass the completion audit. Each merge must have a matching log entry with validation and review evidence.

Inspect the graph and log. Where did autonomy depend on executable constraints rather than prompt wording? Did separate implementer and reviewer roles find different problems? Which stop conditions protected product or architectural decisions from being guessed?

The run log records `Merge: PASS` plus the feature-tip commit. The merge commit's own SHA is reported by Git and the final audit because a commit cannot contain its own hash.


# Five-minute challenge — Make the coordinator's next decision

After the README run has completed or stopped, act as coordinator for these hypothetical situations. Use the stop and integration rules in `AGENTS.md`.

1. Validation is green, but an IMPORTANT review finding remains after the second fix round. The implementer suggests merging and fixing it in the next issue.
2. Validation and review passed on the feature branch. Rebase then changed the feature's base commit, and the implementer proposes reusing the earlier validation result.
3. The next issue says “reject duplicate registrations” without defining what counts as a duplicate. The implementer offers to choose a policy automatically.

For each situation, choose **continue**, **revalidate**, or **stop**. Name the exact next action, whether merging is allowed, and the evidence or decision required before progress is possible.

Then draft a three-line stop-log entry for one situation: reason, evidence, and preserved branch/next required decision. Use clearly marked hypothetical references rather than inventing successful command results.

Deliver your decisions and log entry only. Do not restart the factory, modify its immutable queue, or add an eleventh issue. The challenge is to apply the controls when a shortcut looks tempting.
