# Architecture Step 3 - Fitness Test Green

This step contains the corrected design with the architecture fitness test passing.

## Goal

Compare the final design with the violation from [`03-Architecture/Step1`](../Step1).

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
cd 03-Architecture/Step3
dotnet test ArchitectureFitness.slnx
```

## Expected Learning

The domain layer no longer depends on application or infrastructure modules. HTTP communication is handled by an infrastructure adapter behind an application-owned port.

The infrastructure module also provides the composition registration that connects `IOrderRiskGateway` to `HttpOrderRiskGateway`. The DI test validates that the real object graph can be built without moving those dependencies into the domain model.

The architecture rule is strongest when it is documented for humans and agents, reviewed manually, and enforced through an executable test in CI.
