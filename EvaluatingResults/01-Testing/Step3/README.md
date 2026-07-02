# Testing Step 3 - Good Tests

This step contains the improved test suite and corrected production code.

## Goal

Compare these tests with your review notes from `01-Testing/Step2` and check how the production code was corrected.

## Run

```bash
cd 01-Testing/Step3
dotnet test
```

## Expected Learning

The solution adds the missing boundary tests for 5 kg and 20 kg.

The production code is corrected so standard shipping is below 20 kg and heavy shipping starts at 20 kg.

Green generated tests are not enough if they do not assert the behavior from the prompt.
