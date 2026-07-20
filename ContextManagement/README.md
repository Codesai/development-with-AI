Small API that receives interest records and saves them in interests.txt.

## Requirements

- Use GitHub Codespaces

OR 

- Docker and Docker Compose

## Run application

From the ContextManagement folder start the service with docker-compose:
   docker compose up --build -d

View records on the host in back/interests.txt (it updates as submissions arrive)

## Exercises

Complete the exercises in order:

1. [One-file project instructions](./01-project-instructions-exercise.md): define validation rules in a root `AGENTS.md`, then organize the same rules using modular `AGENTS.md` files.
2. [Skills](./02-skills-exercise.md): create separate frontend and backend validation skills instead of defining the rules in `AGENTS.md` files.
3. [Code is context](./03-code-is-context.md): remove the explicit project instructions and skills, then observe how the existing validation code guides the agent's implementation.
