# 02 - Modular Files Project Instructions

## Goal

Learn to define project specific instructions using modular AGENTS.md 

## Instructions

Create an AGENTS.md file for front and another for back with the associated rules 

Front-end:

- Include all validation functions in a JavaScript file called `validations.js`.
- Do not use standard HTML validations.

Back-end:

- Perform validations in controller classes before calling the repository.
- When validation fails, return an HTTP bad request.

## Recomendations

Check the agent output to see if the files are loaded when we expected.
Try to ask the agent to only do validations in front or back a verify that only the needed rules are loaded into the context.
