# Architecture Step 1 - Find the Violation

This step contains generated code with an architectural violation.

## Goal

Identify where domain code depends on infrastructure concerns.

## Architecture Rule

```text
api -> application -> domain
infrastructure -> application
domain -> no framework dependencies
```

## Run

```bash
cd Architecture/Step1
dotnet build ArchitectureFitness.slnx
```

## Task

- Inspect `src/ArchitectureFitness.Domain/Order/OrderRiskPolicy.cs`.
- Explain why depending on `HttpClient` from the domain layer is dangerous.
- Propose where external HTTP communication should live instead.
- Write one natural-language architectural rule that would prevent this.
