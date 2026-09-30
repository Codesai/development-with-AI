# 03 - Subagent orchestration

## Goal

Sometimes the task we want to delegate to subagents is not completely independent, and we need to provide each subagent with the necessary information to complete the task.

## Instructions

We are going to add new functionality: a field in the registration form where the user can include comments or notes about their interest in the course.

We are going to use the same subagents that we defined in the previous exercise to do the frontend and backend work. But this time, before sending the work to the subagents, we need to define the contract between the client and server.

For this task, we are going to need some form of orchestration, so we are going to create a third subagent to coordinate the work. This "orchestration-agent" needs to define the workflow for the task. Before sending the work to the frontend and backend subagents, the agent needs to understand the current messages between the frontend and backend, create a new version of these messages with the new field, and only then delegate the work to the subagents, giving them information about this new contract.

## Recommendations

You can use `/task` while the agent is working on your request to see the subagents involved in the task and view their individual logs.
 
## Questions

- Do we need a subagent to do the orchestration? Is it the only way, or can you think of other options?
- If the orchestration logic is inside one subagent, can we guarantee that the workflow will always be followed in a deterministic fashion?
