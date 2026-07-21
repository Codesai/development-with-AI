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
- Notice the skipped architecture test and remove `Skip` from its `[Fact]` attribute.
- Run `dotnet test` again to activate the architecture rule.
- Explain which architectural rule the test enforces, which dependency violates it, and which domain type causes the failure.
- Do not fix the production code yet.

### Task 3 - Ask AI to Fix the Violation

- Ask an AI coding assistant to fix the architecture rule violation while preserving the application's behavior.
- Give the assistant the failing test output and the architectural rule as context.
- Review the proposed changes and confirm that dependencies point in the intended direction.
- Run `dotnet test` and verify that the architecture test now passes.
