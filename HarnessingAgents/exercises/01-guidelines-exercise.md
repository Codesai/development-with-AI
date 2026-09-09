# 01 - Guidelines

## Goal

Write a guideline in `AGENTS.md` for writing code without comments, and observe how it steers the way the agent implements a feature.

## The feature

Add a confirmation code to each registration. When a registration is saved, generate a code, store it as a new tab-separated column in `interests.txt`, return it in the API response, and show it in the frontend confirmation message.

Code format: `AAA-YYYYMMDD-NNN-C`

- `AAA` - the first three letters of the course, uppercased and right-padded with `X` if the course name is shorter.
- `YYYYMMDD` - the UTC date the registration was saved.
- `NNN` - how many registrations that course already has for that day, zero-padded to three digits.
- `C` - a check character: sum every digit in `NNN` and `YYYYMMDD`, take it modulo 36, and map `0-35` to `0-9A-Z`.

## Instructions

Use the same prompt for every run:

> Add a registration confirmation code to this project. Implement the feature end to end: generate the code when a registration is saved, store it in `interests.txt`, return it in the API response, and show it in the frontend confirmation message. The code format is `AAA-YYYYMMDD-NNN-C` (course prefix, UTC date, daily per-course sequence, check character).

1. Baseline. Read `app/AGENTS.md`. It asks for readable, intent-revealing code but never says how: that could mean explanatory comments, or it could mean good names and small functions. The heavily commented starter code tips the balance. Start `copilot` in `HarnessingAgents/app` and give it the prompt. Run `make run`, submit a registration, and confirm the code appears.

2. Read the diff. Between the commented starter code and the ambiguous `AGENTS.md`, the agent tends to fill the new feature with `///` docs and `//` step comments.

3. Revert the feature changes, keep `AGENTS.md`.

4. Add a guideline to `app/AGENTS.md` so it forbids comments. The agent should rely on descriptive names and small functions. When having to edit code that already has comments, it should remove them and rename or abstract if necessary.

5. Start a fresh `copilot` session so it reloads `AGENTS.md`, and give it the same prompt.

6. Compare the two implementations. Did the agent write comments for the new code? Did the agent leave or update comments already present in code it had to edit?

## Recommendations

Change one line of `AGENTS.md` at a time and re-run the same prompt.

Ask the agent, "What guidelines does this project have?" to check whether it has access to the `AGENTS.md` file.

Remember you can always revert changes using version control.
