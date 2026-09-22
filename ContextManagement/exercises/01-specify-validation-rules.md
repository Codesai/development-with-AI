# 01 - Project instructions

## Goal

learn different ways to stablish design or architecturl rules for out project

## Instructions

Validate that the email is syntactically valid before persisting the request. The project has these validation rules:

Frontend:

- Include all validation functions in a JavaScript file called `validations.js`.
- Do not use standard HTML validation.

Backend:

- Perform validation in controller classes before calling the repository.
- When validation fails, return an HTTP Bad Request response.

We are going to try 3 different approaches to achieve our goal:
    1 - Define an AGENTS.md field in the root of the project (/ContextManagement/app)
    2 - Define Hierarchichal AGENTS.md files, one in the back folder with the validations rules and the same for the front
        - check in the output of the agent that the files are loaded correctly
    3 - Create two skills one to include the rules for validation in the front and another to include the validations in the back

After finishing each step roolback all changes and remove all new files that you create to start fresh, you can exccute "make rollback" to do that. 

## Recommendations

if the Agent is not applying the rules you can ask the Agent: “What are the validation rules for this project?” to check whether it has access to the file or files containing these rules.

Useful commands for working with skills in Copilot:

- `/skills reload` (skills are only loaded when Copilot starts a session)
- `/skills list`
- `/skills info SKILL-NAME`

Verify in the Copilot CLI output that the required skill is loaded by the Agent when needed

## Questions

- What is the best option to specify these rules in the current team you are working on?
- How can we be sure that the skills are loaded?
- This is gona work with other agents and/or models?