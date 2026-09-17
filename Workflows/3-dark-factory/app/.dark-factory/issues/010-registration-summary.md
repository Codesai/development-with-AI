# Issue 010 — Summarize registrations

## Goal

Expose aggregate counts for the complete registration collection.

## Acceptance criteria

- `GET /api/registrations/summary` returns HTTP 200.
- The response has `total` and `byCourse` counts for `Introduction` and `Advanced`.
- Both course keys are present with zero values when storage is empty.
- Mixed registrations produce the correct total and per-course counts.
- The endpoint reads the repository once per request.
- Appropriate automated checks are included.

## Constraints

- Reuse the shared course definition and asynchronous repository read.
- Do not expose persistence-only fields.
- Do not add dependencies or modify unrelated functionality.

