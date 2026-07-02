# Architecture Step 2 - Fitness Test Red

This step adds an executable architecture fitness test while keeping the violation.

## Goal

Turn the architectural rule into a failing test.

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
cd 03-Architecture/Step2
dotnet test ArchitectureFitness.slnx
```

The test should fail because the domain layer still depends on infrastructure concerns.

## Task

- Read `tests/ArchitectureFitness.ArchitectureTests/DomainArchitectureTest.cs`.
- Compare the test rule with the natural-language rule from `03-Architecture/Step1`.
- Decide whether this rule also belongs in `AGENTS.md`, a code review checklist, CI, or all of them.
