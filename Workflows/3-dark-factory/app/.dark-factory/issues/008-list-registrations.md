# Issue 008 — List stored registrations

## Goal

Allow clients to retrieve the registrations in storage order.

## Acceptance criteria

- `GET /api/registrations` returns HTTP 200 with registration objects containing `name`, `email`, and `course`.
- An empty repository returns an empty JSON array.
- Multiple registrations are returned in storage order.
- Storage timestamps or internal persistence fields are not exposed.
- Appropriate automated checks are included.

## Constraints

- Extend the existing repository abstraction with asynchronous reading.
- Keep file parsing inside the file repository.
- Do not add dependencies or modify unrelated functionality.

