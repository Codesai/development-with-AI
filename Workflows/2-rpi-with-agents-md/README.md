# Exercise 2 — Crystallize the workflow

The .NET 10 registration application now tells Copilot how features should be developed through `AGENTS.md`.

## Learning goal

Move reusable process instructions out of repeated prompts and into the repository's engineering environment.

## Before you start

Run `make validate`, read `AGENTS.md`, and create one clean branch per feature.

## Run and deploy

Start the application with Docker Compose:

```sh
docker compose up --build -d
```

The application is available at `http://localhost:8080`. Registrations are persisted in `back/interests.txt`.

Application folder and file layout:

```text
Dockerfile
docker-compose.yml
back/
  Controller.cs
  Domain.cs
  Repository.cs
  Program.cs
  InterestApi.csproj
  interests.txt
front/
  index.html
  script.js
```

Exercise instructions, prompts, and the Makefile accompany this application layout. The repository interface and implementation share `back/Repository.cs`; dependency injection and registration behavior are preserved.

For the exercise:

1. Launch `copilot` and paste `prompts/feature-a.md`; approve its plan and inspect the result.
2. Return to the starter state on another branch.
3. Launch `copilot` and paste `prompts/feature-b.md`; approve its plan and inspect the result.

Feature A adds an operational endpoint. Feature B adds registration validation.

## Success and reflection

Both runs should follow the same phases and finish with validation evidence even though the prompts do not repeat those rules. Which instructions belong to a task, and which belong to the reusable harness?
