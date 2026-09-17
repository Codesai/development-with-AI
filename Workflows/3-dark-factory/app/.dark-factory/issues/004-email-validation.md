# Issue 004 — Validate registration email addresses

## Goal

Reject registrations whose normalized email is not a valid address.

## Acceptance criteria

- `POST /api/register` returns HTTP 400 for an address without a valid mailbox and host structure.
- Validation is applied after the normalization introduced by Issue 003.
- A representative valid email continues to return HTTP 201 and is persisted.
- Invalid email requests are not persisted.
- Appropriate automated checks are included.

## Constraints

- Use .NET built-in email parsing; do not add a regular-expression or validation dependency.
- Preserve all required-field behavior.
- Do not modify unrelated functionality.

