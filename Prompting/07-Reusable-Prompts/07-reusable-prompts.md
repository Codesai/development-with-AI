# 07 - Reusable Prompts

## Goal

Recognize opportunities to reuse prompts for recurring tasks and explore how a few additional instructions can turn a static prompt into a conversational one.

## The Task

Use both reusable prompts and compare the experience of providing the information each prompt needs.

Pay attention to:

- How much work is required to reuse each prompt
- Which parts remain useful across different features
- How a small prompt change affects the user experience

## Stage 1 — Parameterized static prompt

In this stage, you replace every placeholder before running the prompt.

1. Open [reusable-prompt-template.md](./reusable-prompt-template.md).
2. Copy the prompt into a working document.
3. Find every placeholder marked with square brackets, such as `[FEATURE_NAME]`.
4. Replace the placeholders with the **Password reset parameters** below.
5. Check that no placeholder remains and that you have not changed the fixed instructions.
6. Run the completed prompt in your AI assistant.
7. Review whether the generated story reflects all the parameter values you supplied.
8. Note which parts of preparing and running the prompt were easy, repetitive, or error-prone.

### Password reset parameters

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

## Stage 2 — Conversational reusable prompt

In this stage, you run the prompt without replacing its placeholders. The AI collects the values from you through a conversation.

1. Start a new conversation with your AI assistant.
2. Open [interactive-reusable-prompt.md](./interactive-reusable-prompt.md).
3. Copy only the prompt inside its code block into the conversation.
4. Keep every placeholder unchanged and run the prompt.
5. Check that the AI asks for one missing parameter at a time instead of producing a user story immediately.
6. Answer each question using the **Transaction search parameters** below.
7. Keep a record of the questions, including any that were unclear, repetitive, or missing.
8. If the AI asks for several parameters at once, remind it to ask for one parameter at a time.
9. After the AI generates the story, review whether it used all the information you supplied.
10. Compare the two prompt files and identify the instructions that make the second prompt conversational.
11. Note how those few additional lines changed the experience of reusing the prompt.

### Transaction search parameters

- Business goal: Reduce the time customers spend finding a past transaction
- Feature name: Transaction search
- Primary user: Customer
- User goal: Find a transaction using a keyword and date range
- Reason this feature exists: Customers need to locate past payments without reviewing every statement
- Scope: search within the customer's transaction history
- Included scenarios: searching by keyword, filtering by date range, and viewing matching results
- Excluded scenarios: exporting results, disputing transactions, and searching another customer's account

## Reflection questions

- When you notice that you are prompting for something similar repeatedly, which parts could you keep and reuse?
- How did adding a few conversational instructions change the experience of using the prompt? What does this show about the flexibility of AI?
- When would you prefer the static prompt, and when would you prefer the conversational prompt?
- What other questions could the AI ask to obtain the information needed for an even better result?
