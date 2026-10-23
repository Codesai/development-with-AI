# Workflows – Exercise 1 – Simple prompt vs. structured workflow

## Learning goal

See how explicit research, planning, implementation, and validation phases change an agent's outcome, even for a small feature request.

## Part 1

- Go to the `../app` directory,
- Run `copilot` and enter this prompt:
```text
Add a health check endpoint to this application.
``` 
- Save in a file to compare later: agent log, changed files, added checks, and commands run.

## Part 2

- Revert every change made in Part 1.
- In a new session, launch `copilot` and paste the following prompt:

```text
Add a health check endpoint to this application. Create `GET /api/health`. It must return HTTP 200 and `{"status":"ok"}`.

Workflow:
1. Research the application’s architecture and conventions, and clarify any uncertainty about the feature with me.
2. Wait for my approval.
3. Propose an implementation and testing plan.
4. Wait for my approval.
5. Implement the approved plan.
6. Run the validation checks.
7. Fix any issues found.
8. Report a concise summary, including the relevant details, changed files, and validation results.
```
- When Copilot pauses, review its response and approve it if you agree.

## Success and reflection

Both implementations should provide `GET /api/health`, return HTTP 200 with `{"status":"ok"}`, and preserve registration behavior.

- Compare how Copilot worked with the simple prompt and the structured prompt. Identify which differences came from the workflow.
- How complete were the implemented validation checks?
