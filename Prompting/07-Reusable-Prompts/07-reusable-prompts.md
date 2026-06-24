# 07 - Reusable Prompts

## Goal

Turn the prompt you built in the previous exercises into a reusable prompt template that can create a new user story for different features without rewriting the whole prompt every time.

## Instructions

1. Review the prompt you created across exercises 01 to 06.
2. Identify which parts should stay fixed and which parts should become placeholders.
3. Replace feature-specific details with placeholders.
4. Run the reusable prompt by filling in the placeholders for a new feature.
5. Compare the output with the user story generated in exercise 06.
6. Share your reusable prompt and output with the group.

## The Task

Create a reusable prompt that helps a user write a new user story.

Your reusable prompt should include:

- A role for the AI
- Context about the product, users, and business goal
- The task to create a user story
- Constraints that define the scope
- Format instructions for the output
- An example user story to guide style and level of detail
- Placeholders that can be changed for each new feature

## Reusable prompt template

Use the template in [reusable-prompt-template.md](./reusable-prompt-template.md).

## Example placeholders

Use these values for a first test:

- Product: Online banking app
- Main users: Retail banking customers
- Business goal: Reduce support calls by letting users solve common account access issues themselves
- Feature name: Password reset
- Primary user: Customer
- User goal: Reset a forgotten password
- Reason this feature exists: Customers need to recover access without contacting support
- Scope: customer-facing password reset flow
- Included scenarios: requesting a reset link and setting a new password
- Excluded scenarios: account lockout, fraud review, and support agent workflows

## Reflection questions

- Which parts of the prompt are reusable across features?
- Which placeholders were necessary to avoid rewriting the whole prompt?
- Did the reusable prompt produce a user story with the same quality as exercise 06?
- Did the example still influence the output?
- What placeholder would you add for your own team or project?
- How could this reusable prompt become part of your team's regular workflow?
