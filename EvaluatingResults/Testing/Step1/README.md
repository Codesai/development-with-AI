# Testing Step 1 - Starting Point

This step contains the shipping project before meaningful tests have been added.

## Goal

Use this state to understand the code shape and the prompt before reviewing any generated test suite.

Prompt:

> Implement shipping cost calculation where premium users and non-premium users with packages under 5 kg get free shipping, and packages of 20 kg or more are classified as heavy shipping.

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
