# Testing Step 1 - Starting Point

This step contains the shipping project before meaningful tests have been added.

## Goal

Ask an agent to create tests for the given production code, then examine and comment on the generated tests.

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
- Ask an agent to add tests for the prompt.
- Review the generated tests before accepting them.
- Comment on what the tests cover, what they miss, and whether they give enough confidence.
