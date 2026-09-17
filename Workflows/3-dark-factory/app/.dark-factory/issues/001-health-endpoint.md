# Issue 001 — Add a health endpoint

## Goal

Expose a lightweight endpoint that proves the API process is available.

## Acceptance criteria

- `GET /api/health` returns HTTP 200.
- The JSON response is `{ "status": "ok" }`.
- Calling the endpoint does not persist a registration.
- Appropriate automated checks are included.

## Constraints

- Follow the existing controller and dependency-injection patterns.
- Do not add dependencies or modify registration behavior.
- Do not modify unrelated functionality.

