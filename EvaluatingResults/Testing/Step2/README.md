# Testing Step 2 - AI-Generated Tests

This step contains the AI-generated implementation and test suite.

## Goal

Review the tests as a human reviewer. The tests are green, but the review question is whether they prove the requested behavior.

Prompt:

> Implement shipping cost calculation where premium users and non-premium users with packages under 5 kg get free shipping, and packages of 20 kg or more are classified as heavy shipping.

## Run

```bash
cd Testing/Step2
dotnet test
```

## Task

- Check whether premium users get free shipping.
- Check whether non-premium packages below 5 kg get free shipping.
- Check the boundary at exactly 5 kg.
- Decide which missing tests should block accepting the generated change.
