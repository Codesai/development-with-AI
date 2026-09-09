# Issue 007 — Expose the course catalog

## Goal

Allow clients to discover the courses accepted by registration validation.

## Acceptance criteria

- `GET /api/courses` returns HTTP 200.
- The JSON array contains `Introduction` followed by `Advanced`.
- The endpoint and registration validation use the same course definition.
- Calling the endpoint does not access or modify registrations.
- Appropriate automated checks are included.

## Constraints

- Do not duplicate the offered-course values in controllers.
- Do not add dependencies or modify unrelated functionality.
- Preserve all earlier endpoints and behavior.

