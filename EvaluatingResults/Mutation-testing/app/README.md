# .NET mutation-testing example

A small API in .NET 10, that receives interest records and saves them to `interests.txt` together with their registration status.

It also contains a focused domain rule, `RegistrationPolicy`, used to demonstrate how mutation testing finds weaknesses that ordinary passing tests can miss.

## Requirements

- Use GitHub Codespaces

OR 

- Docker and Docker Compose

## Run the application

From this directory, start the container:

```bash
make run
```

Open <http://localhost:8080>. Submitted registrations are persisted in `back/interests.txt` through the Docker volume.

## Registration policy

`RegistrationPolicy` is intentionally pure domain logic, which makes it a compact target for unit and mutation tests.

| Condition | Decision |
| --- | --- |
| Course is closed, or terms are not accepted | `Rejected` |
| 0–30 | `Accepted` |
| 31+ | `Waitlisted` |

The form requires accepting the terms. Every submission is saved with an `Accepted`,
`Waitlisted`, or `Rejected` status in the fifth `interests.txt` column. Capacity is
calculated independently for each course from accepted registrations only.


## Run the tests

```bash
make test
```

The tests run with code coverage (coverlet). The report is written to `coverage-report/index.html` (opened automatically in the default browser; in GitHub Codespaces it is served on port 8000 and opened through the forwarded port, stop the server with `make stop-report`), and a text summary is printed in the terminal. The report also includes the cyclomatic complexity and the CRAP score of each method (open a class page and look at the *Crap Score* column, or the *Risk Hotspots* section in `index.html`).

## Run mutation testing

```bash
make mutation-test
```

This target runs the unit tests first, then Stryker. Stryker mutates only `back/RegistrationPolicy.cs` and writes an HTML report under `StrykerOutput/`. 

## Static analysis and combined report

```bash
make analyze   # Roslyn / Sonar analyzers -> analysis-report/index.html
make verify    # analysis + tests + mutation testing, then opens reports/index.html
```

The analyzers (.NET Analyzers and SonarAnalyzer.CSharp, configured in `Directory.Build.props`) run on every build. `make analyze` collects their findings as SARIF logs and renders them with `scripts/sarif_to_html.py`. `make verify` links the coverage, static analysis and mutation reports from a single landing page. `make clean` removes all generated reports.
