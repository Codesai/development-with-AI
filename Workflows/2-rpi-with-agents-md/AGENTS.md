# Engineering Workflow

Use this workflow for every feature change. Keep the change focused, preserve existing behavior, and do not weaken validation to make a check pass.

## Feature Workflow

1. **Research**: Before editing, inspect the relevant .NET and browser code. Identify existing patterns and briefly explain them. Consider at least two implementation options, present the options to the user, and wait for approval.
2. **Plan**: Write a short implementation plan and wait for approval.
3. **Implement**: Make only the changes required for the approved feature. Preserve valid behavior and add focused automated tests. All validations, old and new, should be run with `make validate`.
4. **Review**: Ask a separate subagent in a new agent session to review the changes. Report findings under `HIGH`, `MEDIUM`, and `LOW` severity. 
5. **Fix**: Fix all `HIGH` and `MEDIUM` findings without expanding the feature before continuing  scope. Re-run the relevant tests after each fix.
6. **Validate**: Run the complete available validation, including `make validate`, and record the results. Do not hide, loosen, or remove a failing check.
7. **Report**: Summarize the changed files and provide the exact validation results.

## Testing

- Use a .NET test framework.
- Follow the test pyramid: prefer focused unit tests, supported by a smaller number of integration tests.
- Add unit tests for domain use cases, covering the service-to-infra-interface behavior.
- Add integration tests covering the controller through in-memory persistence.
- Keep tests deterministic and independent of external services.
