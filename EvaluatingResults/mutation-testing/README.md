# Front/back .NET mutation-testing example

A small API in .NET 10, that receives interest records and saves them to `interests.txt`.

## Requirements

- Use GitHub Codespaces

OR 

- Docker and Docker Compose

## Run the application

From the `ContextManagement` directory, start the service with:

```bash
make run
```

The application is available at <http://localhost:8080>.

View records on the host in back/interests.txt (it updates as submissions arrive)

## Policy and boundary

The course capacity is 30:

| Confirmed registrations | Decision |
| --- | --- |
| 0–29 | `Accepted` |
| 30+ | `Waitlisted` |
| Course closed or terms not accepted | `Rejected` |

## Exercise

Run Stryker and see what mutants survive. 

Stryker mutates only `back/RegistrationPolicy.cs` and writes its HTML report to `StrykerOutput`.


```bash
cd mutation-testing
dotnet test
dotnet stryker
```

See report in ./StrykerOutput directory.
