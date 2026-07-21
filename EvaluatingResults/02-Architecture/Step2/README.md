# Architecture Step 2 - Fitness Test Red

This step adds an executable architecture fitness test while keeping the violation.

## Goal

Turn the architectural rule into a failing test.

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
dotnet test ArchitectureFitness.slnx
```

The test should fail because the domain layer still depends directly on an outer module.

## Task

- Read [`tests/ArchitectureFitness.ArchitectureTests/DomainArchitectureTest.cs`](tests/ArchitectureFitness.ArchitectureTests/DomainArchitectureTest.cs).
- Compare the test rule with the natural-language rule from [`02-Architecture/Step1`](../Step1).
- Decide whether this rule also belongs in `AGENTS.md`, a code review checklist, CI, or all of them.
