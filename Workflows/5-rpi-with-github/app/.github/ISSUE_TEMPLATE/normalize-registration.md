---
name: CI feedback-loop exercise
about: Protect course-interest data at the storage boundary
title: "Reject unsafe registration fields before persistence"
labels: enhancement
---

## Feature

Prevent field content from corrupting the line-oriented, tab-separated registration file.

## Acceptance criteria

- Add a formatter operation that reports whether a registration is safe to store.
- Reject fields containing tabs, carriage returns, or newlines.
- Accept ordinary non-empty registration fields.
- Preserve trimming and the tab-separated storage format.
- Add focused local checks for safe and unsafe registrations.
- Local validation and full CI must pass without weakening checks.
