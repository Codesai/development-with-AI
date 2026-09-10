# Additional challenge — From a big plan to vertical slices

## 1. Start with a big feature

Start from the starter application on a new branch. Paste this prompt:

> Turn this registration form into a course discovery experience. Replace the course dropdown with a list of courses, each with a short description, a way to select it, and a link to its own landing page so visitors can learn more before registering. Each landing page should include learning outcomes, the intended audience, prerequisites, a detailed syllabus, teaching format, duration, instructor information, and frequently asked questions. Let visitors compare courses and register their interest from any course page with that course already selected. Make the experience work well on mobile and allow visitors to share direct links to individual courses. Use sample content where information is missing.

Approve a research approach and let the agent produce a plan. **Stop before implementation.** Keep the plan for comparison.

## 2. Teacher checkpoint: “This is too big”

Discuss the plan's size, assumptions, and time to deliver something useful.

The teacher introduces **vertical slicing**: each slice delivers a usable outcome across the layers it needs. Database, API, and UI tasks alone are not vertical slices.

Ask the agent:

> This plan is too big. Split it into vertical slices. For each, explain the user outcome, scope, deferred work, and how to verify it. Do not implement yet.

Can the first slice be used without the rest?

## 3. Update AGENTS.md

Add reusable workflow rules covering:

- When to propose slicing.
- BIG, MEDIUM, and SMALL options, each with a slice count, user outcomes, and tradeoffs.
- Usable, demonstrable, and independently verifiable slices.
- User selection of granularity and approval of the first slice’s plan.
- Implementation, review, and validation of only the approved slice.

Keep feature details in the prompt. Preserve research and plan approvals and all quality checks.

## 4. Try again

Start a fresh session with the updated `AGENTS.md` and unchanged starter application. Repeat the original prompt.

Does the agent offer slice granularities? Compare with the first plan and refine your rules if needed.

## 5. Implement one slice

Choose a granularity, agree on the first slice's outcome and acceptance examples, and approve its plan. Ask:

> Implement only the first agreed slice. Leave the remaining slices for later.

Complete the review and validation workflow, then demonstrate the user outcome.

## Deliverables

- Original plan and proposed slices.
- Updated `AGENTS.md` and fresh-session granularity options.
- One working slice with acceptance evidence and validation results.

What can the user do now? What is deferred? Would your rules work for another feature?

## Preserving the plan

How can we preserve the plan and agreed slices so we can continue in a new agent session?
