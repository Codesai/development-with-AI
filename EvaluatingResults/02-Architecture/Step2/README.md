# Architecture Step 2 - Fitness Test Green

This step contains the corrected design with the architecture fitness test passing.

## Goal

Compare the final design with the violation from [`02-Architecture/Step1`](../Step1).

## Architecture Rule

The project follows this layered architecture:

```text
infrastructure -> application
application -> domain
domain -> no application or infrastructure module dependencies
```

The infrastructure module owns external technical details such as HTTP clients.

## Run

```bash
cd 02-Architecture/Step2
dotnet test
```

## Expected Learning

The domain layer no longer depends on application or infrastructure modules. HTTP communication is handled by an infrastructure adapter behind an application-owned port.

The architecture rule is strongest when it is documented for humans and agents, reviewed manually, and enforced through an executable test in CI.
