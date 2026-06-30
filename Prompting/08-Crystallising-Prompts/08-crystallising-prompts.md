# 08 - Crystallising Prompts

## Goal

Take a reusable prompt template and turn it into a deterministic tool: a script that asks you for the parameters and generates the output directly — without AI.

## Instructions

1. Run the script:
   ```bash
   ./generate-user-story.sh
   ```
2. Answer each question when prompted.
3. The script will assemble the user story from your answers, no AI involved.
4. Compare the output with the one you got in exercise 07.

## Example values

Use these values to try it out:

- Product name: Online banking app
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

- What does the script do that the template alone could not?
- Is user story creation a good fit for this approach? What feels forced?
- What kind of task would be a better candidate for this pattern?
- At what point does a task become structured enough to no longer need AI?
