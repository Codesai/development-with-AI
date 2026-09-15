# Front/back .NET mutation-testing example

This folder is a standalone example; it does not change the existing `Architectural-fitness` application. It uses the same course-interest context as that app, but adds a small front end and minimal API around a domain policy that is easy to mutate and test.

## Critique of `01-Test-Review-And-Mutation/Step2`

The Shipping example is excellent for its narrow lesson: tests may pass while `<` versus `<=` boundary mutants survive. It is less suitable here because it has no front/back flow, no domain-specific outcome, and no architectural role for the rule. Its three happy-path tests also make the learning outcome almost predetermined.

This proposal retains the boundary lesson but gives it a realistic home. `RegistrationPolicy` is pure domain logic; the API maps requests to it, and the front end calls the API. Controllers need not duplicate capacity rules, and mutation testing stays focused on the domain file.

## Policy and boundary

The course capacity is 30:

| Confirmed registrations | Decision |
| --- | --- |
| 0–29 | `Accepted` |
| 30+ | `Waitlisted` |
| Course closed or terms not accepted | `Rejected` |

The tests deliberately cover 29 and 30. That kills the most important mutation: changing `< CourseCapacity` to `<= CourseCapacity`.

## Run

```bash
cd Architectural-fitness/mutation-testing
dotnet test
dotnet run --project backend
dotnet stryker
```

Open the URL printed by `dotnet run`, submit the form with 29 and then 30 confirmed registrations, and see the different API decisions.

Stryker mutates only `backend/Domain/RegistrationPolicy.cs` and writes its HTML report to `StrykerOutput`.

## Exercise

Remove the test case for exactly `RegistrationPolicy.CourseCapacity`, then run Stryker. The `<=` boundary mutant should survive. Restore the test and run it again to show why green tests alone did not prove the rule.
