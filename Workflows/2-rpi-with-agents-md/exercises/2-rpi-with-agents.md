# Workflows - Exercise 2 — Crystallize the workflow

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

Move reusable process instructions out of repeated prompts and into the repository's engineering environment.

## Before you start

Read `AGENTS.md`, and create one clean branch per feature:

```bash
git checkout -b ex-w-2a
git checkout ex-w-2b
```

The application currently saves `Name`, `Email`, and `Course` through `POST /api/register`; there is no health endpoint.

## Exercises


1. Launch `copilot` and paste `prompts/feature-a.md`; approve the research and plan and inspect the result.
2. Return to the starter state on another branch `b`.
3. Launch `copilot` and paste `prompts/feature-b.md`; approve the research and plan and inspect the result.

Feature A adds a field. Feature B prevents duplicate registrations.

## Success and reflection

Both runs should follow the same phases and finish with validation evidence even though the prompts do not repeat those rules. 
Which instructions belong to a task, and which belong to the reusable harness?


# Ex 2 - Additional challenge — From a big plan to vertical slices

This challenge builds on exercise 2 in `2-rpi-with-agents-md/`.

**For students and the teacher:** keep this guide outside the agent's exercise workspace. It reveals the workflow changes you will introduce later, which could influence the agent's initial plan. Start the agent from `2-rpi-with-agents-md/`, and share only the prompts below at the indicated steps. Do not attach this guide or ask the agent to read it.

## 1. Start with a big feature

Start from the starter application on a new branch. Paste this prompt:

> Turn this registration form into a course discovery experience. Replace the course dropdown with a list of courses, each with a short description, a way to select it, and a link to its own landing page so visitors can learn more before registering. Each landing page should include learning outcomes, the intended audience, prerequisites, a detailed syllabus, teaching format, duration, instructor information, and frequently asked questions. Let visitors compare courses and register their interest from any course page with that course already selected. Make the experience work well on mobile and allow visitors to share direct links to individual courses. Use sample content where information is missing.

Follow the research and planning approval steps, and let the agent produce a plan. **Stop before implementation.** Save the plan for comparison before asking for changes.

## 2. Teacher checkpoint: “This is too big”

Discuss the plan's size, assumptions, and time to deliver something useful.

The teacher introduces **vertical slicing**: each slice delivers a usable outcome across the layers it needs. Database, API, and UI tasks alone are not vertical slices.

Ask the agent:

> This plan is too big. Split it into vertical slices. For each, explain the user outcome, scope, deferred work, and how to verify it. Do not implement yet.

Can the first slice be used without the rest?

## 3. Update AGENTS.md

Add reusable workflow rules covering:

- When to propose slicing.
- BIG, MEDIUM, and SMALL slice-size options, each with a slice count, user outcomes, and tradeoffs.
- Usable, demonstrable, and independently verifiable slices.
- User selection of granularity and approval of the first slice’s plan.
- Implementation, review, and validation of only the approved slice.

Keep feature details in the prompt. Preserve research and plan approvals and all quality checks.

## 4. Try again

Start a fresh agent session in the exercise directory with the updated `AGENTS.md` and unchanged starter application. Repeat the original prompt from step 1, without sharing the earlier plan or this guide.

Does the agent offer different slice sizes without being asked explicitly? Compare its proposal with the first plan and refine your rules if needed.

## 5. Implement one slice

Choose a slice size, agree on the first slice's outcome and acceptance examples, and approve its plan. Ask:

> Implement only the first agreed slice. Leave the remaining slices for later.

Complete the review and validation workflow, then demonstrate the user outcome.

## Group discussion

- What are the key differences between the original plan and the proposed slices?
- How did the updated `AGENTS.md` change the slice-size options and decision-making process?
- Is the first proposed slice good enough to deliver value on its own?
- Would these rules still work for a different feature or product area?

## Preserving the plan

How can we preserve the plan and agreed slices so we can continue in a new agent session?
