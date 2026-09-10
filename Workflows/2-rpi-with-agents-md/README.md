# Workflows - Exercise 2 — Crystallize the workflow

A small API in .NET 10, that receives interest records and saves them to `interests.txt`.

## Requirements

- Use GitHub Codespaces

OR 

- Docker and Docker Compose

## Run the application

From this directory, start the service with:

```bash
make run
```

The application is available at <http://localhost:8080>.

View records on the host in back/interests.txt (it updates as submissions arrive)

## Learning goal

Move reusable process instructions out of repeated prompts and into the repository's engineering environment.

## Before you start

Read `AGENTS.md`, and create one clean branch per feature:

```bash
git checkout -b ex-w-2a
git checkout ex-w-2b
```

The application currently saves `Name`, `Email`, and `Course` through `POST /api/register`; there is no health endpoint.

## Exercises


1. Launch `copilot` and paste `prompts/feature-a.md`; approve the research and plan and inspect the result.
2. Return to the starter state on another branch `b`.
3. Launch `copilot` and paste `prompts/feature-b.md`; approve the research and plan and inspect the result.

Feature A adds a field. Feature B prevents duplicate registrations.

## Success and reflection

Both runs should follow the same phases and finish with validation evidence even though the prompts do not repeat those rules. 
Which instructions belong to a task, and which belong to the reusable harness?
