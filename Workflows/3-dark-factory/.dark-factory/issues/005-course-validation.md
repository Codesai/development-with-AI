# Issue 005 — Validate offered courses

## Goal

Accept registrations only for courses that the application offers.

## Acceptance criteria

- The only accepted normalized course values are `Introduction` and `Advanced`.
- Course comparison is exact after the normalization introduced by Issue 003.
- An unknown or differently cased course returns HTTP 400 and is not persisted.
- Both offered courses return HTTP 201 when the rest of the request is valid.
- Appropriate automated checks are included.

## Constraints

- Define the offered values in one application-level location that later endpoints can reuse.
- Do not add dependencies or modify unrelated functionality.
- Preserve all earlier validation behavior.

