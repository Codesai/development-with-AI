# Security review

## Summary

I inspected the application under `ContextManagement/app` and found two actionable issues.

| # | Severity | File | Lines | Vulnerability | Confidence |
|---|----------|------|-------|---------------|------------|
| 1 | 🟡 MEDIUM | ContextManagement/app/back/Program.cs | 5-9 | Unauthenticated registration API with unrestricted CORS | 9/10 |
| 2 | 🟡 MEDIUM | ContextManagement/app/back/Repository.cs | 17-22 | Unsanitized user input written directly to a text file | 8/10 |

## Finding 1: Unauthenticated registration endpoint with unrestricted CORS

- `Program.cs` enables CORS with `AllowAnyOrigin()`, `AllowAnyHeader()`, and `AllowAnyMethod()`.
- `Controller.cs` exposes a public `POST /api/register` endpoint without authentication or authorization.
- This allows any external site or script to submit arbitrary registration payloads and potentially poison or spam the application.

### Recommended remediation

- Restrict CORS to trusted origins.
- Require authentication or a signed token for all state-changing requests.
- Add CSRF protection for browser-based clients if cookies or session auth are used.

## Finding 2: Unsanitized user input written directly to file

- `Repository.cs` appends untrusted `Name`, `Email`, and `Course` values directly into `interests.txt`.
- The code does not validate format, length, or escaping.
- Attackers can inject separators, control characters, or HTML-like content, which could create malformed records or stored XSS in downstream rendering.

### Recommended remediation

- Validate and constrain each field before saving.
- Replace raw TSV text storage with a structured format.
- Escape or encode data before display or export.

## Notes

The application is intentionally small and the issues are straightforward to remediate. The highest-priority fix is to restrict state-changing API access and validate all submitted data before persisting it.
