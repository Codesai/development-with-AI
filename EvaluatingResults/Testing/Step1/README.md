# Testing Step 1 - Starting Point

This step contains the shipping project before meaningful tests have been added.

## Goal

Use this state to understand the code shape and the prompt before reviewing any generated test suite.

Prompt:

> Implement free shipping for premium users and for non-premium users when the package weighs less than 5 kg.

## Run

```bash
cd Testing/Step1
dotnet test
```

The existing test is only a placeholder. It should not give confidence in the shipping rules.

## Task

- Read `src/Shipping/ShippingCostCalculator.cs`.
- Read `tests/Shipping.Tests/ShippingCostCalculatorTest.cs`.
- Identify which behaviors need real tests before trusting a generated change.
