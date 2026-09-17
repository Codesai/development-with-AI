# Issue 006 — Prevent duplicate course registrations

## Goal

Avoid storing the same person more than once for the same course.

## Acceptance criteria

- A second registration with the same normalized email and course returns HTTP 409.
- Email matching for duplicate detection is case-insensitive.
- The duplicate request does not create a second stored record.
- The same email may register once for each different offered course.
- Appropriate automated checks are included.

## Constraints

- Extend the repository contract only as much as duplicate detection requires.
- Keep duplicate policy in the application layer rather than the file repository.
- Do not introduce a database, dependency, or unrelated refactor.

