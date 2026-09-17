# Issue 009 — Filter registrations by course

## Goal

Allow the registration list to be narrowed to one course.

## Acceptance criteria

- `GET /api/registrations` continues to return every registration in storage order.
- `GET /api/registrations?course=Advanced` returns only exact `Advanced` matches, preserving storage order.
- Either offered course with no matches returns an empty array.
- An unknown or differently cased course also returns an empty array, not an error.
- Appropriate automated checks are included.

## Constraints

- Apply filtering in the application layer after one repository read.
- Reuse the shared offered-course definition where relevant.
- Do not add dependencies or modify unrelated functionality.

