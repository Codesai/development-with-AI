# Workflows - Exercise 1 - Turn an ad-hoc request into a workflow

## Learning goal

See how explicit research, planning, implementation, and validation phases change an agent's outcome, even for a small feature request.

## Part 1

### Part 1.A

Go to the `../app` directory, and on the `main` branch, launch `copilot` and enter this prompt:

```text
Add a health check endpoint to this application.
``` 

Pay attention to the agent log, changed files, added checks, and commands run.

### Part 1.B

- Revert every change made in Part 1.A.
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

Press <kbd>Enter</kbd>. When Copilot pauses, review its response and approve it if you agree.

## Success and reflection

Both implementations should provide `GET /api/health`, return HTTP 200 with `{"status":"ok"}`, and preserve registration behavior.

Compare how Copilot worked with the simple prompt and the structured prompt. The feature is the same (`GET /api/health`); identify which differences came from the workflow. 

How complete were the implemented validation checks?

## Part 2

Continue on `main` and complete the tasks in order. For each task:

1. Write a prompt that includes the workflow from the previous exercise.

2. Consider whether every phase is necessary for the task, or whether some can be relaxed. Explain your decision.

3. Take note of the decisions made, changed files, and validation results before moving to the next task.

Choose the next task only after completing the preceding one:

| Task | Feature request | Focus |
| --- | --- | --- |
| 2.1 | Add an optional comments field to the registration form. Save it with each registration. | Full-stack field addition: form, API model, and storage format. |
| 2.2 | Show the submitted comment in the confirmation message after a successful registration. | API response contract and UI feedback. |
| 2.3 | Allow a visitor to opt in to receive course updates. Save their choice with the registration. | Checkbox behavior, Boolean defaults, persistence, and backward compatibility. |
| 2.4 | Add a page that lists saved registrations, with the newest registrations first. | Read path, endpoint design, persisted-data parsing, and rendering. |
| 2.5 | Allow users to filter the registration list by course. | Query parameters, client-side state, and empty-result behavior. |
| 2.6 | Let an administrator download the filtered registration list as a CSV file. | Export format, escaping commas and newlines, HTTP headers, and filter consistency. |
| 2.7 | Add a registration-details page that can be opened from the list. | Stable registration identity, routing, and not-found behavior. |
| 2.8 | Allow an administrator to delete a registration from its details page after confirmation. | Destructive actions, API design, persistence rewrite, and error handling. |
| 2.9 | Make the registration list update automatically when a new registration is created. | Polling versus server push, consistency, lifecycle management, and error handling. |
