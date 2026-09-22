# 03 - Subagents

## Goal

Understand the role of subagents and try the subagents support in copilot cli.

## Instructions

if you have changes from the previous exercise please execute 'make roolback' in the folder /ContextManagement/App

We are going to do the same task than in the previous exercises, email validation, but in this case using 
subagents. 

inside the folder /ContextManagement/App (start copilot from this folder):

define two subagents:
    - one to work with the frontend code
        - for example create the file -> .github/agents/front-dev.agent.md
    - the second to work with the backend code
        - for example create the file -> .github/agents/back-dev.agent.md

- Verify it the subagents are correctly identified by copilot cli using the comand "/subagents"

test the diferent mechanisms in cli to delegate task to the subagents:

- to use one agent you can type /agent <agent-name> the copilot session switch to this subagent and the next prompts are executed by the subagent

- copilot infers automatically if there is a custom defined agent suitable for the task and delegates the task in any normal prompt (but sometimes decide to not delegate... for example for small tasks)

- you can indicate explicitly in the prompt when you want to delegate some task to some subagents, for example "do the email validation and use frontend-dev to the changes in the front"

- with the command "/fleet" you can send a prompt with an implementation plan and copilot divide this plan in subtask than can be delegated to subagents and executed in parallel. if you want a subtask to use a specfic agent use @agent-name, for example "/fleet validate the email, use @frontend-dev for the fornt validations and @backend-dev for the validations in the back"


## Recomendations

you can use /task while Agent is working on your request to see the subagentes involved in the task and to view individual logs for each subagent
 
## questions

- this rules are enought to enforce the system generate the code the way we want
- If we want to enforce some validations rules what is the best place for them?
    - inside the new dedicated agent?
    - using skills?
    - using AGENTS.md?

