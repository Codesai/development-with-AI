# Architecture Step 3 - Fitness Test Green

This step contains the corrected design with the architecture fitness test passing.

## Goal

Compare the final design with the violation from `03-Architecture/Step1`.

## Architecture Rule

The project follows this layered architecture:

```text
api -> application
application -> domain
domain -> no framework dependencies
```

## Run

```bash
cd 03-Architecture/Step3
dotnet test ArchitectureFitness.slnx
```

## Expected Learning

The domain layer no longer performs external HTTP communication directly.

The architecture rule is strongest when it is documented for humans and agents, reviewed manually, and enforced through an executable test in CI.
