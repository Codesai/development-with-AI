# Architecture Step 1 - Find the Violation

This step contains generated code with an architectural violation.

## Goal

Identify where domain code depends on infrastructure concerns.

## Architecture Rule

The project follows this layered architecture:

```text
api -> application
application -> domain
application -> infrastructure
domain -> no framework dependencies
```

## Run

```bash
cd 03-Architecture/Step1
dotnet build ArchitectureFitness.slnx
```

## Task

- Inspect `src/ArchitectureFitness.Domain/Order/OrderRiskPolicy.cs`.
- Explain if you see any problem and make a proposal to fix it.
- Write one natural-language architectural rule that would prevent this.
