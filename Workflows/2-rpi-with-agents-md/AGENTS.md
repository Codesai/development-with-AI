# Config

## Feature workflow

1. RESEARCH PHASE: Research relevant .NET and browser code before editing, identify existing .NET Core patterns and explain them briefly. Explore more than one option to approach the implementation, present to user and wait for approval.
2. PLAN PHASE: Write a short plan and wait for approval.
3. IMPLEMENTATION PHASE: Implement changes and keep them within the requested feature. Preserve valid behavior. Add focused tests, as automated validation. Never weaken validation to make it pass. The implent should pass validation as `make validate`.
4. REVIEW PHASE: Launch a new subagent a make a code review for the code. Create a report with findings, classified in three categories: HIGH, MEDIUM, LOW. Is mandatory fix HIGH and MEDIUM. Return to main agent.
5. FIX PHASE: Fix code review findings if applied. Apply all config of implementation phase
6. VALIDATE PHASE: Pass all validations.
5. FINAL: Report changed files and exact results.


## Test

Use a .NET test framework
Create tests applying the pyramid test.
Create unit test for use-case, from service to interface, inside domain.
Create integration tests from controller to in-memory persistance.
