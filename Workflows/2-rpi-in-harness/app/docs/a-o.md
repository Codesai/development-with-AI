Workflow:

1. **Research**: Before editing, inspect the relevant .NET and browser code. Identify existing patterns and briefly explain them. **Consider at least two implementation options, present the options to the user, and wait for approval.**
2. **Plan**: Write a short implementation plan and wait for approval.
3. **Implement**: Make only the changes required in plan for the approved feature. Preserve valid behavior and add focused automated tests. Follow criteria in [## Testing](#Testing). All validations, old and new, should be run with `make validate`.
4. **Review**: **Ask a separate subagent in a new agent session to review the changes. Report findings under `HIGH`, `MEDIUM`, and `LOW` severity.**
5. **Fix**: **Fix all `HIGH` and `MEDIUM` findings without expanding the feature before continuing  scope. Re-run the relevant tests after each fix.**
6. **Validate**: Run the complete available validation, including `make validate`, and record the results. Do not hide, loosen, or remove a failing check.
7. **Report**: Summarize the changed files and provide the exact validation results.
