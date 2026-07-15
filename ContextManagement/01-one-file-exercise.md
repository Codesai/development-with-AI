# 01 - One file project instructions

## Goal

Learn to define project specific instructions using one AGENTS.md in the root of the project

## Instructions

We want to validate that the email is syntactically valid before persists the request, for our project we have some rules for validations:

Front-end:
    Include all the validations functions in a javascript file called validations.js 
    Don’t use standard html validations
Back-end:
    The validations are done in the controller classes before calling the repository 
    When a validation fails, return an HTTP bad request

## Recomendations

Try to create one rule at a time, look at the generated code after creating the rule, and if it's what you expected, move on to the next one
Try to ask the IA “what are the rules for validation in this project” if we want to check is the agent has access to the file with the rules.
