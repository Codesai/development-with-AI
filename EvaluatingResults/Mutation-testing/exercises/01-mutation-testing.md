# Exercise 1 — Mutation testing

## Goal

Learn how mutation testing evaluates the effectiveness of a test suite, identify a
behaviour that the current tests do not verify, and improve the tests so that they
detect the surviving mutant.

## Registration policy

The application records a person's interest in a course. `RegistrationPolicy`
contains the domain rules that decide the registration status, making it a small,
focused target for mutation testing.

| Condition | Decision |
| --- | --- |
| The course is closed, or the terms have not been accepted | `Rejected` |
| There are 0–30 confirmed registrations | `Accepted` |
| There are 31 or more confirmed registrations | `Waitlisted` |

Every submission is saved with an `Accepted`, `Waitlisted`, or `Rejected` status
in the fifth column of `interests.txt`. Capacity is calculated independently for
each course and counts accepted registrations only.

## Instructions

Move to the application directory. All paths and commands in this exercise are
relative to that directory:

```bash
cd ../app
```

### 1. Understand the behaviour and its tests

Read these files before running anything:

- `back/RegistrationPolicy.cs`
- `tests/RegistrationDemo.Api.Tests/RegistrationPolicyTests.cs`
- `stryker-config.json`

Compare the policy table above with the test cases. Pay particular attention to
the boundary at which an accepted registration becomes waitlisted.

### 2. Establish the baseline

Run the existing test suite:

```bash
make test
```

Confirm that every test passes. Passing tests establish a baseline, but they do
not prove that the suite would detect meaningful changes to the implementation.

### 3. Run mutation testing

Run Stryker:

```bash
make mutation-test
```

The target runs the unit tests first and then mutates only
`back/RegistrationPolicy.cs`. Review the terminal summary and open the generated
`StrykerOutput/<timestamp>/reports/mutation-report.html` report in a browser.

In the report:

1. Find the mutant with the `Survived` status.
2. Compare its replacement expression with the original implementation.
3. Explain why all the existing tests still pass with that mutation.

Do not change the production code: it already implements the policy described
above.

### 4. Strengthen the test suite

Add the smallest test case needed to distinguish the original condition from the
surviving mutation. Name the test after the behaviour it verifies, rather than
after the implementation detail or the mutant.

Run the unit tests and mutation tests again:

```bash
make test
make mutation-test
```

Confirm that the new test passes against the production code, the previously
surviving mutant is now killed, and the mutation score has improved.

## Discussion

- What weakness did mutation testing reveal that a green test suite did not?
- Why are values immediately around a decision boundary especially useful in
  tests?
- How is mutation score different from code coverage?
- Should every surviving mutant result in a new test? Why or why not?
