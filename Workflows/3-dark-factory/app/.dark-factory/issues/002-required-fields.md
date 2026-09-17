# Issue 002 — Reject missing registration fields

## Goal

Prevent incomplete registrations from being stored.

## Acceptance criteria

- `POST /api/register` rejects a null, empty, or whitespace-only `Name`, `Email`, or `Course` with HTTP 400.
- A rejected request is not persisted.
- A request with all three fields populated continues to return HTTP 201.
- Each required-field rule has an isolated automated check.

## Constraints

- Keep validation outside the repository implementation.
- Do not add dependencies or weaken existing checks.
- Do not modify unrelated functionality.

