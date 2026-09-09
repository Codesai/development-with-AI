Small API that receives interest records and saves them in interests.txt.

## About this exercise set

A harness is the system you put around a coding agent so its output stays aligned with what you value: behaviour, style, architecture, security, and process. A harness is built from:

- Guidelines - feedforward controls given to the agent before it decides (`AGENTS.md`, skills, templates, glossaries, the code itself).
- Guardrails - feedback controls that run after a change (linters, static analysis, tests, review scripts, another agent or a human reviewing).
- Gateways - controlled entry points that define what the agent is even able to do (custom commands, a Makefile as the only way to build, an MCP as the only way to edit).

These exercises use one throwaway feature per experiment as the vehicle. The feature is not the point; the harness is.

## Requirements

- Use GitHub Codespaces

OR 

- Docker and Docker Compose

## Run application

From the HarnessingAgents folder start the service with:
   make run

View records on the host in back/interests.txt (it updates as submissions arrive)

## Exercises

1. [Guidelines](../HarnessingAgents-docs/01-guidelines-exercise.md): write a guideline in `AGENTS.md` for writing code without comments, and observe how it steers the way the agent implements a feature.
