# HarnessingAgents

Exercises for building your own harness around a coding agent: the controls you put in place so its output stays aligned with what you value - behaviour, style, architecture, security, and process.

A harness is built from:

- Guidelines - feedforward controls given to the agent before it decides (`AGENTS.md`, skills, templates, glossaries, the code itself).
- Guardrails - feedback controls that run after a change (linters, static analysis, tests, review scripts, another agent or a human reviewing).
- Gateways - controlled entry points that define what the agent is even able to do (custom commands, a Makefile as the only way to build, an MCP as the only way to edit).

Each exercise uses one throwaway feature as the vehicle. The feature is not the point; the harness is.

## Layout

- `app/` - a small .NET 10 API that stores interest registrations. This is the only folder the agent works in. Launch `copilot` here and run `make run` here (Docker and Docker Compose, or Codespaces).
- `exercises/` - the exercise instructions.
- `harness/` - guardrail scripts and other harness pieces the student installs.

`exercises/` and `harness/` sit outside `app/` on purpose. Several exercises only work if the agent does not know what is being tested: if it can read the exercise brief or a guardrail script, it just complies up front and you never see the control do its work. Keep `copilot` in `app/` and keep the rest out of its reach.

## Exercises

1. [Guidelines](exercises/01-guidelines-exercise.md): write a guideline in `AGENTS.md` for writing code without comments, and observe how it steers the way the agent implements a feature.
2. [Guardrails](exercises/02-guardrails-exercise.md): wire a comment-checking script into a Copilot CLI `postToolUse` hook and re-run the exercise 01 feature to see the guardrail correct the agent after each edit.
