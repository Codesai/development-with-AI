# Exercise 1 - Review an AI-Generated Change

## Goal

Practice human-in-the-loop code review.

## Duration

20 minutes

## Setup

Use the prepared evaluation branches. Students do not need to copy or create the code manually.

Start from the AI-generated change:

```bash
git switch evaluation/tests/02-my-ai-generated-tests
```

The branch contains the generated implementation and generated tests for this prompt:

> Implement free shipping for premium users and for non-premium users when the package weighs less than 5 kg.

Available branches:

- `evaluation/tests/01-no-tests`: exercise starting point without meaningful tests.
- `evaluation/tests/02-my-ai-generated-tests`: AI-generated implementation and tests to review.
- `evaluation/tests/03-complete-review-solution`: completed review solution for comparison after the exercise.
