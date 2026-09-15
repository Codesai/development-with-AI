# Exercise 2 - Architectural Fitness

## Goal

Practice how we can ensure that the AI (or human) generated code conforms with the architecture rules that we 
have in our project

## Instructions

### Exercise 2.1

Using architecture test (deterministic):

    - Check the file /back/test/architecture/ArhitectureTests.cs to understand the architectural restrictions that we want
to preserve

    - Execute this test in the terminal 'dotnet test back/test/architecture/InterestApi.ArchitectureTests.csproj'

    - Check the results and try to understand why the architectural rules are violated. Before asking the AI think in posible
solutions to the problems.

    - Start copilot, ask copilot to execute the test an propose solutions to fix the architectural violations. You can ask 
copilot to propose a solutions without touching any file, compare with your solution and after that decide the way to go.

    - After the solution is implemented execute the test to ensure the architecturla integrity

### Exercise 2.2

Using the architectural code review skill (inference):

    - Check the file .github/skills/architectural-code-review to see how the architecture rules are expresed in the skill

    - open copilot and execute the skill (with the original version if code, y you do any changes in the previous steps revert those changes before). 

    - Check if the skill is capabable to detect the same violations that the architectural test detect in the previous step

## Questions

- We see two ways to detect architecture violations, discuss with your partners about the pro's and con's of the two options
