# Architecture Step 1 - Find the Violation

This step contains generated code with an architectural violation.

## Goal

Identify where domain code depends on infrastructure concerns.

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
cd 02-Architecture/Step1
dotnet build ArchitectureFitness.slnx
```

## Task

- Inspect [`src/ArchitectureFitness.Domain/Order/OrderRiskPolicy.cs`](src/ArchitectureFitness.Domain/Order/OrderRiskPolicy.cs).
- Notice that [`src/ArchitectureFitness.Infrastructure/Orders/HttpOrderRiskGateway.cs`](src/ArchitectureFitness.Infrastructure/Orders/HttpOrderRiskGateway.cs) is an infrastructure adapter that uses HTTP.
- Explain if you see any problem and make a proposal to fix it.
- Write one natural-language architectural rule that would prevent this.
