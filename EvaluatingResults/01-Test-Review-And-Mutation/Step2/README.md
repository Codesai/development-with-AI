# Step 2 - Tool-Assisted Review (Mutation Testing)

This step uses [Stryker.NET](https://stryker-mutator.io/docs/stryker-net/introduction/) to evaluate whether the AI-generated test suite can detect small faults (mutants) introduced into the production code.

## Goal

Automate test evaluation using a tool to verify the observations from your manual review in Step 1.

## Setup

Verify that Stryker.NET is installed:

```bash
dotnet stryker --version
```

If it is not installed, install it globally:

```bash
dotnet tool install --global dotnet-stryker
```

## Predict Mutants

Before running Stryker, predict whether the current AI tests would fail for each mutation listed below:

| Mutation | Would tests fail? |
| --- | --- |
| Change `weightKg < 5.0` to `weightKg <= 5.0` | ? |
| Change `weightKg < 20.0` to `weightKg <= 20.0` | ? |
| Change `return 9.99` to `return 4.99` | ? |
| Change `isPremium || weightKg < 5.0` to `isPremium && weightKg < 5.0` | ? |
| Change `return 4.99` to `return 0.0` | ? |

## Run Stryker

From the `EvaluatingResults` directory, run:

```bash
cd 01-Test-Review-And-Mutation/Step2
dotnet test
dotnet stryker
```

## Expected Results Comparison

| Mutation | Would tests fail? | Meaning |
| --- | --- | --- |
| Change `weightKg < 5.0` to `weightKg <= 5.0` | **No (Survives)** | 5 kg boundary not tested |
| Change `weightKg < 20.0` to `weightKg <= 20.0` | **No (Survives)** | 20 kg boundary not tested |
| Change `return 9.99` to `return 4.99` | **Yes (Killed)** | Heavy package cost is tested |
| Change `isPremium || weightKg < 5.0` to `isPremium && weightKg < 5.0` | **Yes (Killed)** | Premium logic is tested |
| Change `return 4.99` to `return 0.0` | **Yes (Killed)** | Standard shipping cost is tested |

## Key Takeaway

Green tests and high code coverage are not enough. A line of code can be executed without the tests being strong enough to catch boundary faults. Mutation testing tools automatically expose the gaps identified during manual review.

## 🏆 Challenge: Fix it!

Try to **fix the tests yourself**:
1. Add the missing boundary tests to `tests/Shipping.Tests/ShippingCostCalculatorTest.cs`.
2. Run `dotnet test`. Do they pass? If they fail, why? (Hint: Does the AI implementation match the prompt's rules precisely?)
3. Fix the production code if needed.
4. Run `dotnet stryker` again and aim for **0 surviving mutants**.
