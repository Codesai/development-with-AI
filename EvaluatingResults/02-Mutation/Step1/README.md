# Mutation Step 1 - Find Surviving Mutants

This step contains the AI-generated implementation, the AI-generated tests, and the Stryker.NET configuration.

## Goal

Think like a mutation testing tool before running it.

## Run Tests

```bash
cd 02-Mutation/Step1
dotnet test
```

The tests should be green before starting the exercise.

## Predict

For each mutation, decide whether the current tests would fail.

| Mutation | Would tests fail? |
| --- | --- |
| Change `weightKg < 5.0` to `weightKg <= 5.0` | ? |
| Change `weightKg < 20.0` to `weightKg <= 20.0` | ? |
| Change `return 9.99` to `return 4.99` | ? |
| Change `isPremium || weightKg < 5.0` to `isPremium && weightKg < 5.0` | ? |
| Change `return 4.99` to `return 0.0` | ? |

## Run Stryker

```bash
dotnet stryker
```

The `stryker-config.json` file already selects the solution, production project, test project, and mutation target.

If `dotnet stryker` is not installed:

```bash
dotnet tool install --global dotnet-stryker
```

Use the generated `StrykerOutput` report to compare the real result with your predictions.
