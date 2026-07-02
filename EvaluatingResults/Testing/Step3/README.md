# Testing Step 3 - Review Solution

This step contains the completed review solution.

## Goal

Compare this test suite with your review notes from `Testing/Step2`.

## Run

```bash
cd Testing/Step3
dotnet test
```

## Expected Learning

The solution adds tests for the missing non-premium free-shipping rule and the 5 kg boundary, and corrects the implementation to match "less than 5 kg".

Green generated tests are not enough if they do not assert the behavior from the prompt.
