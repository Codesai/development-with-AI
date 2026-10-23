# Config

## Feature Workflow

Use this workflow for every feature change:

1. **Research**: Before editing, inspect the relevant .NET and browser code. Identify existing patterns and briefly explain them. **Consider at least two implementation options, present the options to the user, and wait for approval.**
2. **Plan**: Write a very detailed implementation plan (contracts between frontend and backend, API endpoints, database schema changes, public methods, etc.) and wait for approval.
3. **Implement**: Make only the changes required in plan for the approved feature. Preserve valid behavior and add focused automated tests. Follow criteria in [Testing](#testing). All validations, old and new, should be run with `make validate`.
4. **Review**: Ask a separate subagent to review the changes. New agent session with a different model than main.
5. **Fix**: Fix all relevant findings without expanding the feature before continuing scope. Re-run the relevant tests after each fix.**
6. **Validate**: Run the complete available validation, including `make validate`, and record the results. Do not hide, loosen, or remove a failing check.
7. **Report**: Summarize the changed files and provide the exact validation results.


## Testing

- Use a .NET test framework.
- Follow the test pyramid: prefer focused unit tests, supported by a smaller number of integration tests.
- Add unit tests for domain use cases, covering the service-to-infra-interface behavior.
- Add integration tests covering the controller through in-memory persistence.
- Keep tests deterministic and independent of external services.
- Follow FIRST principles.
