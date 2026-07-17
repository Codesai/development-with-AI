# 01 - One file project instructions

## Goal

Learn to define project-specific instructions using project instructions. 

1 - Create one Agents.md file in the root of ContextManagement with the validation rules
2 - Try to define the same rules but this time using modular Agents.md files

## Instructions

Validate that the email is syntactically valid before persisting the request. The project has these validation rules:

Frontend:

- Include all validation functions in a JavaScript file called `validations.js`.
- Do not use standard HTML validation.

Backend:

- Perform validation in controller classes before calling the repository.
- When validation fails, return an HTTP Bad Request response.

## Recommendations

Create an AGENTS.md file in ContexMangement root

Create one rule at a time. Review the generated code after creating each rule; if it meets your expectation, move on to the next one.

Ask the AI, “What are the validation rules for this project?” to check whether it has access to the file containing the rules. 

Remenber you can always revert changes using version control 


