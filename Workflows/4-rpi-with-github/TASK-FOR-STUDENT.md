# Five-minute challenge — Decide what CI evidence justifies

After completing the README tutorial, consider two hypothetical reports for a later PR:

**A:** Local validation passes. CI says: `expected Ada\tada@example.com\tAdvanced, got Ada\tADA@EXAMPLE.COM\tAdvanced`.

**B:** Local validation passes. CI cannot download an SDK dependency because the package service returns HTTP 503; no application check runs.

For each report, write a short triage note naming:

- what the evidence establishes and what it does not;
- the next agent action and whether an application-code change is justified;
- what evidence would allow a reviewer to accept the result.

For A, include a concrete local regression-check input and expected output. For B, explain what to report if the third CI run ends the same way.

Deliver at most twelve lines. This is a reasoning exercise: do not trigger more CI runs, change checks, or merge the PR. Retain the three-run boundary even when the failure is outside the application.
