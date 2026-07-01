# Exercise 2 - Mutation Testing

## Goal

Understand why green tests can be weak.

## Duration

25 minutes

## Step 1 - Think Like a Mutation Testing Tool

Switch to the branch for this exercise:

```bash
git switch evaluation/mutation/01-finding-mutants
cd EvaluatingResults
dotnet test
```

This branch contains the AI-generated implementation, the AI-generated tests, and the Stryker.NET configuration. The tests should be green before starting the exercise.

For each mutation, decide whether the current tests would fail.

| Mutation | Would tests fail? |
| --- | --- |
| Change `weightKg <= 5.0` to `weightKg < 5.0` | ? |
| Change `weightKg <= 20.0` to `weightKg < 20.0` | ? |
| Change `return 9.99` to `return 4.99` | ? |
| Change `isPremium || weightKg <= 5.0` to `isPremium && weightKg <= 5.0` | ? |
| Change `return 4.99` to `return 0.0` | ? |

## Step 2 - Run a Real Mutation Testing Tool

Run Stryker.NET from the `EvaluatingResults` directory:

```bash
dotnet stryker
```

The exercise branch includes a `stryker-config.json` file, so students can run the tool without choosing the solution, test project, production project, or mutation target manually.

Use the mutation report to compare the real result with your predictions from step 1. The HTML report is generated under `StrykerOutput`.

If `dotnet stryker` is not installed, install it first:

```bash
dotnet tool install --global dotnet-stryker
```

## Student Tasks

1. Predict which mutations survive.
2. Run the mutation testing tool.
3. Compare the report with your predictions.
4. Identify which missing tests would kill the surviving mutations.
5. Decide whether the green AI-generated test suite gives enough confidence.
