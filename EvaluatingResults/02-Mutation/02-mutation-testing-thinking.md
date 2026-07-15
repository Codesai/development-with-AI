# Exercise 2 - Mutation Testing Thinking Exercise

## Goal

Understand why green tests can be weak.

## Duration

25 minutes

## Setup

Use the prepared folders in `main`. Students do not need to copy or create the code manually.

These exercises use [Stryker.NET](https://stryker-mutator.io/docs/stryker-net/introduction/) to evaluate whether tests can detect small faults (mutants) introduced into the production code. Install Stryker.NET once as a global .NET tool in the environment where you run the exercises, including a Codespaces terminal:

```bash
dotnet tool install --global dotnet-stryker
```

If it is already installed, update it instead:

```bash
dotnet tool update --global dotnet-stryker
```

Verify that the tool is available:

```bash
dotnet stryker --version
```

If your shell cannot find `dotnet stryker` after installation, open a new terminal so it picks up the .NET global-tools path.

## Steps

- [`02-Mutation/Step1`](Step1): predict surviving mutations, run the tests, and run Stryker.NET. Read [`02-Mutation/Step1/README.md`](Step1/README.md).
- [`02-Mutation/Step2`](Step2): compare your predictions with the expected answers and learning notes. Read [`02-Mutation/Step2/README.md`](Step2/README.md).

Each step includes a `stryker-config.json` file that selects the solution, production project, test project, and mutation target. Run the following from a step directory:

```bash
dotnet test
dotnet stryker
```

Stryker writes an HTML report to `StrykerOutput` in the step directory. Open the report to inspect killed and surviving mutants.

The goal is to see why a green AI-generated test suite can still miss important faults.
