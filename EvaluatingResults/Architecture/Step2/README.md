# Architecture Step 2 - Fitness Test Red

This step adds an executable architecture fitness test while keeping the violation.

## Goal

Turn the architectural rule into a failing test.

## Run

```bash
cd Architecture/Step2
dotnet test ArchitectureFitness.slnx
```

The test should fail because the domain layer still depends on infrastructure concerns.

## Task

- Read `tests/ArchitectureFitness.ArchitectureTests/DomainArchitectureTest.cs`.
- Compare the test rule with the natural-language rule from `Architecture/Step1`.
- Decide whether this rule also belongs in `AGENTS.md`, a code review checklist, CI, or all of them.
