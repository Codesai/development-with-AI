# 02 - Modular Files Project Instructions

## Goal

Learn to define project-specific instructions using modular `AGENTS.md` files.

## Instructions

Create an `AGENTS.md` file for the frontend and another for the backend with the associated rules.

Frontend:

- Include all validation functions in a JavaScript file called `validations.js`.
- Do not use standard HTML validation.

Backend:

- Perform validation in controller classes before calling the repository.
- When validation fails, return an HTTP Bad Request response.

## Recommendations

Check the agent output to see whether the files are loaded when expected.

Ask the agent to perform validation only in the frontend or backend, and verify that only the needed rules are loaded into context.
