# Exercise 1 — Turn an ad-hoc request into a workflow

This .NET 10 interest-registration application mirrors the Codesai Context Management example.

## Learning goal

See how explicit research, planning, implementation, and validation phases change an agent's result even when the requested feature is small.

## Before you start

Run `make validate`. Create two branches from the same starter commit so Parts A and B remain comparable. The application currently saves `Name`, `Email`, and `Course` through `POST /api/register`; there is no health endpoint.

## Part A — Ad-hoc

On the first branch, launch `copilot` and ask: `Add a health check endpoint to this application.` Record files changed, checks added, and commands run.

## Part B — Structured

On the second branch, launch `copilot` and paste `prompts/structured.md`. Approve the plan when Copilot pauses.

## Success and reflection

Both solutions should provide `GET /api/health` with HTTP 200 and `{"status":"ok"}` while preserving registration. Which differences came from the workflow rather than the feature?
