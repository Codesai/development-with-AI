# Reusable Prompt Template

Copy this template, replace the placeholders, and run it:

```text
You are a product owner writing clear, sprint-ready user stories for a software development team.

Product context:
- Product: [PRODUCT_NAME]
- Main users: [USER_TYPES]
- Business goal: [BUSINESS_GOAL]

Feature context:
- Feature name: [FEATURE_NAME]
- Primary user: [PRIMARY_USER]
- User goal: [USER_GOAL]
- Reason this feature exists: [FEATURE_REASON]

Task:
Write one user story for the feature above.

Constraints:
- Keep the story focused on [SCOPE].
- Include only behavior needed for [INCLUDED_SCENARIOS].
- Exclude [EXCLUDED_SCENARIOS].
- Do not include implementation details unless they are necessary to understand the expected behavior.
- Do not create multiple user stories.

Output format:
- Title
- User Story in this structure: "As a... I want... so that..."
- Acceptance Criteria using Given / When / Then
- Definition of Done

Use this example for style, structure, and level of detail:

# Example User Story

## User Story INV-004
As an accountant, I want to see a list of all invoices sent by my company in a given period, so that I can calculate the total revenue for the quarter

## Acceptance Criteria
**Scenario: Accountant retrieves invoices for a quarter**
Given the accountant has access to the invoicing module
When they request the list of invoices for Q1 2026
Then they see a list of all invoices sent in that period
And each invoice shows the amount, date, and status

## Definition of Done
- Acceptance criteria scenarios pass
- Code reviewed by at least one peer
- Deployed to production
```
