# Architecture Step 1 - Find and Detect the Violation

This step contains generated code with an architectural violation.

## Goal

Identify where domain code depends on infrastructure concerns, express the boundary as a rule, and inspect how the rule is enforced automatically.

## Architecture Rule

The project follows this layered architecture:

```text
infrastructure -> application
application -> domain
domain -> no application or infrastructure module dependencies
```

The infrastructure module owns external technical details such as HTTP clients.

## Tasks

### Task 1 - Define the Architectural Rule

- Inspect [`src/ArchitectureFitness.Domain/Order/OrderRiskPolicy.cs`](src/ArchitectureFitness.Domain/Order/OrderRiskPolicy.cs) and the project dependencies.
- Notice that [`src/ArchitectureFitness.Infrastructure/Orders/HttpOrderRiskGateway.cs`](src/ArchitectureFitness.Infrastructure/Orders/HttpOrderRiskGateway.cs) is an infrastructure adapter that uses HTTP.
- **Write one natural-language architectural rule** that would prevent the domain layer from depending on application or infrastructure.

### Task 2 - Detect the Offending Dependency

- Inspect the provided [`tests/ArchitectureFitness.ArchitectureTests/DomainArchitectureTest.cs`](tests/ArchitectureFitness.ArchitectureTests/DomainArchitectureTest.cs).

- Go to `cd 02-Architecture/Step1`
- Run tests: `dotnet test`.
- Explain which architectural rule the test enforces, which dependency violates it, and which domain type causes the failure.
- Do not fix the production code yet.
