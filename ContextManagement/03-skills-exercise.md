# 03 - Skills

## Goal

Lear how to create a skill to include coding standardars or rules

## Instructions

Instead of defining the validations rules in AGENTS.md files we want to create two skills:
    front-validation
    back-validation

Front-end rules:

- Include all validation functions in a JavaScript file called `validations.js`.
- Do not use standard HTML validations.

Back-end rules:

- Perform validations in controller classes before calling the repository.
- When validation fails, return an HTTP bad request.
 

## Recomendations

Useful commands to work with skills in copilot:
    /skills reload (skills are only loaded when copilot start a session)
    /skills list
    /skills info SKILL-NAME

Verify with copilot cli output that the skill is loaded when needed
