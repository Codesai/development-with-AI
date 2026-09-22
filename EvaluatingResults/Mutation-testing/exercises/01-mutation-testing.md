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

## Run mutation testing

```bash
make mutation-test
```

This target runs the unit tests first, then Stryker. Stryker mutates only `back/RegistrationPolicy.cs` and writes an HTML report under `StrykerOutput/`. 
