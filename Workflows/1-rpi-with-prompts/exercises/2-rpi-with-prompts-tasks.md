# Workflows – Exercise 2 – Adapt the workflow to each task

## Learning goal

Learn to decide which phases of the research, plan, implement, and validate workflow a task really needs, and which can be relaxed.

- For this part **reset work is not needed.** You can continue over the work done in [Exercise 1](1-rpi-with-prompts.md). 
- Complete the tasks below in order.
- For each task:

1. Write a prompt that includes the feature to do and the workflow just seen.
2. But before:
   - Ask yourself if we need all the phases for the task, or whether some can be relaxed. 
   - Explain your decision.

Prompt:
```text
<Place here the feature request to do>

Workflow:
<TODO: Ask yourself if we need all the phases for the task>
1. Research the application’s architecture and conventions, and clarify any uncertainty about the feature with me.
2. Wait for my approval.
3. Propose an implementation and testing plan.
4. Wait for my approval.
5. Implement the approved plan.
6. Run the validation checks.
7. Fix any issues found.
8. Report a concise summary, including the relevant details, changed files, and validation results.
```

3. Take note of the decisions made, changed files, and validation results before moving to the next task.

Tasks:

| Task | Feature request | Focus |
| --- | --- | --- |
| 2.1 | Add an optional comments field to the registration form. Save it with each registration. | Full-stack field addition: form, API model, and storage format. |
| 2.2 | Show the submitted comment in the confirmation message after a successful registration. | API response contract and UI feedback. |
| 2.3 | Control duplicated emails. | Prevent duplications |

==========

OPTIONAL:

| 2.4 | Allow a visitor to opt in to receive course updates. Save their choice with the registration. | Checkbox behavior, Boolean defaults, persistence, and backward compatibility. |
| 2.5 | Add a page that lists saved registrations, with the newest registrations first. | Read path, endpoint design, persisted-data parsing, and rendering. |
| 2.6 | Allow users to filter the registration list by course. | Query parameters, client-side state, and empty-result behavior. |
| 2.7 | Let an administrator download the filtered registration list as a CSV file. | Export format, escaping commas and newlines, HTTP headers, and filter consistency. |
| 2.8 | Add a registration-details page that can be opened from the list. | Stable registration identity, routing, and not-found behavior. |
| 2.9 | Allow an administrator to delete a registration from its details page after confirmation. | Destructive actions, API design, persistence rewrite, and error handling. |
| 2.10 | Make the registration list update automatically when a new registration is created. | Polling versus server push, consistency, lifecycle management, and error handling. |
