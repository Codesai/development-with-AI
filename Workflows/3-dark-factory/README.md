# Workflows - Exercise 3 — Run a local Dark Factory

A small API in .NET 10, that receives interest records and saves them to `interests.txt`.

## Requirements

- Use GitHub Codespaces

OR 

- Docker and Docker Compose

## Run the application

From the `ContextManagement` directory, start the service with:

```bash
make run
```

The application is available at <http://localhost:8080>.

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
