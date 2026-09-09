# Workflows - Exercise 1 - Turn an ad-hoc request into a workflow

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

See how explicit research, planning, implementation, and validation phases change an agent's result even when the requested feature is small.

## Before you start

Create two branches from the same starter commit so Parts A and B remain comparable. 

```bash
git checkout -b ex-w-1a
git checkout ex-w-1b
```

The application currently saves `Name`, `Email`, and `Course` through `POST /api/register`; there is no health endpoint.

## Part A — Ad-hoc

On the first branch, launch `copilot` and write this prompt: 

```text
Add a health check endpoint to this application.
``` 

Record files changed, checks added, and commands run.

## Part B — Structured

On the second branch, launch `copilot` and paste: `prompts/structured.md`. 
Approve when Copilot pauses.

## Success and reflection

Both solutions should provide `GET /api/health` with HTTP 200 and `{"status":"ok"}` while preserving registration. 

Which differences came from the workflow rather than the feature?
