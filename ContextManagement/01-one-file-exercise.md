# 01 - One file project instructions

## Goal

Learn to define project-specific instructions using one `AGENTS.md` file in the project root.

## Instructions

Validate that the email is syntactically valid before persisting the request. The project has these validation rules:

Frontend:

- Include all validation functions in a JavaScript file called `validations.js`.
- Do not use standard HTML validation.

Backend:

- Perform validation in controller classes before calling the repository.
- When validation fails, return an HTTP Bad Request response.

## Recommendations

Create one rule at a time. Review the generated code after creating each rule; if it meets your expectation, move on to the next one.

Ask the AI, “What are the validation rules for this project?” to check whether it has access to the file containing the rules.
