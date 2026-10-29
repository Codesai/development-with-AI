# Evaluation example


A small .NET 10 app that receives course interest registrations and saves them to `interests.txt`.

## Requirements

- GitHub Codespaces

or

- Docker and Docker Compose

## Run the application

```bash
make run    # Start the app at http://localhost:8080
make stop   # Stop the app
make clean  # Remove generated reports and build output
```

## Registration policy

`RegistrationPolicy` contains only domain logic, making it a compact target for unit and mutation tests.

| Condition | Decision |
| --- | --- |
| Course is closed, or terms are not accepted | `Rejected` |
| 0–30 | `Accepted` |
| 31+ | `Waitlisted` |

The form requires users to accept the terms. Every submission is saved with an `Accepted`,
`Waitlisted`, or `Rejected` status in the fifth `interests.txt` column. Capacity is
calculated independently for each course, counting only accepted registrations.


## Run the full evaluation

```bash
make verify
```

This command runs the complete evaluation:

- Static analysis
- Functional tests with coverage
- Mutation testing
- Architecture tests
- AI code review

It then opens `reports/index.html`, which links to all six reports.

Build failures, functional test failures or Stryker failures stop the evaluation. If architecture tests fail, the AI review and report generation continue before the command exits with an error. AI review findings do not cause the command to fail.

The evaluation requires .NET 10, Make and Python 3. AI review also requires the GitHub Copilot CLI to be installed and authenticated. In Codespaces, reports are served on port 8000; use `make stop-report` to stop the server.

To remove all generated reports, run `make clean`.


## What is evaluated

### Static analysis

`make analyze`

.NET and Sonar analyzers inspect the API and tests for potential bugs, code smells and maintainability issues.

### Functional tests

`make test`

Unit tests check registration decisions, integration tests check file persistence, and API end-to-end tests check requests, responses and saved registrations.

### Code coverage

Included in `make test`

Code coverage measures the lines and branches exercised by functional tests. The report also shows complexity and CRAP scores to highlight risky code. Coverage alone does not prove correctness.

### Mutation testing

`make mutation-test`

Stryker modifies `RegistrationPolicy.cs` to check whether the tests detect changes in behavior. Surviving mutants reveal possible gaps in the tests.

### Architecture tests

`make architecture-test`

Architecture tests check dependency rules between the controllers, repository and domain. The current controller-to-repository dependency violates one of these rules.

### AI code review

`make code-review`

Copilot's code-review agent inspects the C# implementation and tests for errors and warnings. Its findings can vary and require human review.
