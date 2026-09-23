# 03 - Subagents orchestration

## Goal

Sometimes the task we want to delegate to subagents are not completly independat and we need to provide each subagent the necesary information to complete the task.

## Instructions

We are going to add a new funcionality, we want a new field in the registration form for the user to include some comments or notes about the interest in the course.

We are going to use the same subagents that we define in the previous exercise to do the front and backend work. But this time before sending the work to the subagentes we need to define the contract between client and server before sending the individual task to the subagents.

For this task we are going to require some form of orchestration, we are going to create a third subagent to coordinate this work. this "orchestration-agent" need to define the workflow to do task, before sending the work to the front a back subagents the agent need to undestand the current messages between front and back, create a new version of these messages with the new field and only them delegates the work in the dev's subagents giving them the information about this new contract.

## Recomendations

you can use /task while Agent is working on your request to see the subagents involved in the task and to view individual logs for each subagent
 
## questions

- We need a subagent to do the orchestration?, its the only way or you can think in other options?
- If the orchestration logic is inside one subagent can we garantee that the workflow is gona be followed always in a determinist fashion?