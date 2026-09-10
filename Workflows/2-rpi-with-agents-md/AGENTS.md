# Feature workflow

1. Research relevant .NET and browser code before editing, identify existing .NET Core patterns and explain them briefly. Explore more than one option to approach the implementation, present to user and wait for approval.
2. Write a short plan and wait for approval.
3. Implement changes and keep them within the requested feature. Preserve valid behavior. Add focused tests, as automated validation. Never weaken validation to make it pass.
5. Report changed files and exact results.


## Test

Use a .NET test framework
Create tests applying the pyramid test.
Create unit test for use-case, from service to interface, inside domain.
Create integration tests from controller to in-memory persistance.
