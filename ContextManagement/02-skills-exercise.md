# 03 - Skills

## Goal

Learn how to create a skill that includes coding standards or rules.

## Instructions

Instead of defining the validation rules in `AGENTS.md` files, create two skills:

- `front-validation`
- `back-validation`

Frontend rules:

- Include all validation functions in a JavaScript file called `validations.js`.
- Do not use standard HTML validation.

Backend rules:

- Perform validation in controller classes before calling the repository.
- When validation fails, return an HTTP Bad Request response.
 

## Recommendations

Useful commands for working with skills in Copilot:

- `/skills reload` (skills are only loaded when Copilot starts a session)
- `/skills list`
- `/skills info SKILL-NAME`

Verify in the Copilot CLI output that the required skill is loaded when needed.
