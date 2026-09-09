# Issue 003 — Normalize registrations before storage

## Goal

Store a stable representation of registration input.

## Acceptance criteria

- Leading and trailing whitespace is removed from `Name`, `Email`, and `Course` before persistence.
- The persisted email is converted to invariant lowercase.
- The input object is not mutated.
- Successful requests continue to return HTTP 201.
- Automated checks inspect the exact value passed to the repository.

## Constraints

- Normalize once at the application boundary.
- Use only .NET built-in functionality.
- Do not modify unrelated functionality.

