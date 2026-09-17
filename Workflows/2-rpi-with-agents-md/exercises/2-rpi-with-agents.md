# Workflows - Exercise 2 - Workflow in the Harness

## Learning goal

See how harness instructions provide a reusable workflow without repeating it in every feature prompt.

## Part 1

### Before you start

Read the workflow in [../app/AGENTS.md](../app/AGENTS.md) and compare it with the workflow in [Exercise 1](../../1-rpi-with-prompts/exercises/1-rpi-with-prompts.md).

### Part 1.A

Go to the `../app` directory, launch `copilot`, and paste this prompt:

```text
Add a 'comments' field to form.
```

The agent will follow the workflow in `AGENTS.md`: review the research and options, approve the chosen option, then review and approve the plan.

Inspect the implementation, and record the review findings, changed files, and validation results.

### Part 1.B

In a new Copilot session, paste this prompt:

```text
Prevent duplicate interest registrations.
```

Approve the research option and plan if you agree. Then inspect the result and record the findings, changed files, and validation results.

## Success and reflection

Both runs should follow the same research, planning, implementation, review, fix, validation, and reporting phases, even though neither feature prompt repeats those rules.

Do you consider the RPI defined in `AGENTS.md` useful for both features? Is it especially useful for one of them, or does it fail to add enough value in either case?

Support your answer with the work performed, decisions made, and validation results.

## Part 2 - Additional challenge - From a big plan to vertical slices

Explore what happens when a feature request is too large to implement as a single plan, and how reusable vertical-slicing guidance changes the agent's proposal.

### Part 2.A - Start with a big feature

In a new Copilot session, paste this prompt:

```text
Turn this registration form into a course discovery experience. Replace the course dropdown with a list of courses, each with a short description, a way to select it, and a link to its own landing page so visitors can learn more before registering. Each landing page should include learning outcomes, the intended audience, prerequisites, a detailed syllabus, teaching format, duration, instructor information, and frequently asked questions. Let visitors compare courses and register their interest from any course page with that course already selected. Make the experience work well on mobile and allow visitors to share direct links to individual courses. Use sample content where information is missing.
```

Follow the research and planning approval steps, but stop before implementation. Save the plan for comparison.

### Part 2.B - Checkpoint: “This is too big”

Discuss the plan's size, assumptions, risks, and time needed to deliver something useful.

Introduce **vertical slicing**: each slice delivers a usable outcome across the layers it needs; database, API, and UI tasks alone are not vertical slices.

Ask the agent:

```text
This plan is too big. Split it into vertical slices. Each slice delivers a usable outcome across the layers it needs. For each, explain the user outcome, scope, deferred work, and how to verify it. Do not implement yet.
```

Consider whether the first slice provides value to the user and can be used without the rest.

### Part 2.C - Update `AGENTS.md`

Update `AGENTS.md` and add reusable workflow rules that cover:

- When to propose slicing.
- BIG, MEDIUM, and SMALL slice-size options, each with a slice count, user outcomes, and tradeoffs.
- Usable, demonstrable, and independently verifiable slices.
- User selection of granularity and approval of the first slice's plan.
- Implementation, review, and validation of only the approved slice.

Keep feature details in the prompt. Preserve the research and plan approvals and all quality checks.

### Part 2.D - Try again

Start a fresh Copilot session and repeat the original prompt from Part 2.A. Do not share the earlier plan or this guide.

Does the agent offer different slice sizes without being asked? Compare its proposal with the first plan and refine the rules if needed.

### Part 2.E - Implement one slice

Choose a slice size, approve one of the slicing options and its plan, then ask:

```text
Implement only the first agreed slice. Leave the remaining slices for later.
```

Complete the review and validation workflow, then manually test the user outcome.

## Reflection

- What are the key differences between the original plan and the proposed slices?
- How did the updated `AGENTS.md` change the slice-size options and decision-making process?
- Is the first proposed slice valuable on its own?
- Would these rules work for a different feature or product area?
- How can you preserve the plan and agreed slices to continue in a new agent session?
