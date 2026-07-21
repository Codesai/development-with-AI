# Exercise 1 - Review AI-Generated Tests & Mutation Testing

## Goal

Learn how to evaluate AI-generated tests through **manual code review** (Step 1) and **tool-assisted mutation testing** (Step 2), before validating the **complete solution** (Step 3).

## Setup

Stryker.NET is used in Step 2 and Step 3 to evaluate test strength. Verify it is installed:

```bash
dotnet stryker --version
```

If it is not installed, install it globally:

```bash
dotnet tool install --global dotnet-stryker
```

## Prompt evaluated in this exercise

> Implement shipping cost calculation where premium users and non-premium users with packages under 5 kg get free shipping, and packages of 20 kg or more are classified as heavy shipping.

## Steps

1. [`01-Test-Review-And-Mutation/Step1`](Step1): **Manual Test Review**
   - Review AI-generated code and green tests manually.
   - Check boundary tests and evaluate whether green tests give sufficient confidence.
   - Read [`Step1/README.md`](Step1/README.md).

2. [`01-Test-Review-And-Mutation/Step2`](Step2): **Tool-Assisted Review (Mutation Testing)**
   - Predict surviving mutants on the AI-generated tests.
   - Run Stryker.NET (`dotnet stryker`) to automatically expose weak tests and surviving mutants.
   - Read [`Step2/README.md`](Step2/README.md).

3. [`01-Test-Review-And-Mutation/Step3`](Step3): **Post-Mutation Solution (Good Tests)**
   - Inspect the final test suite with added boundary tests (5 kg, 20 kg) and corrected implementation.
   - Re-run tests and Stryker to confirm 100% mutant kill rate.
   - Read [`Step3/README.md`](Step3/README.md).
