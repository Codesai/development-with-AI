# Exercise 2 - Architectural Fitness

## Goal

Practice how to ensure that AI (or human) generated code conforms to the architecture rules defined in the project.

## Instructions

Move to the `../app` directory. All paths below are relative to that `app` folder.

### Exercise 2.1

Using architecture tests (deterministic):

- Check the file `back/test/architecture/ArchitectureTests.cs` to understand the architectural constraints we want to preserve.
- Run this test in the terminal:
  `dotnet test back/test/architecture/InterestApi.ArchitectureTests.csproj`
- Review the results and try to understand why the architectural rules are being violated. Before asking the AI for help, think about possible solutions to the problems.
- Start Copilot and ask it to run the test and propose solutions to fix the architectural violations. You can ask it to propose a solution without modifying any files, compare its answer with your solution, and then decide which approach to follow.
- Once the solution is implemented, run the test again to confirm the architectural integrity is preserved.

### Exercise 2.2

Using the architectural code review skill (inference):

- Check the file `.github/skills/architectural-code-review` to understand how the architecture rules are expressed in the skill.
- Revert the changes made in step 1.
- Start Copilot and run the skill: `/architectural-code-review`
- Check whether the skill is able to detect the same violations identified by the architectural test in the previous step.

## Questions

- We have two ways to detect architecture violations. Discuss with your partners the pros and cons of each option.
