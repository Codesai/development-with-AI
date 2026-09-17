using InterestApi.Controllers;
using InterestApi.Domain;
using InterestApi.Repository;
using Microsoft.AspNetCore.Mvc;

var repository = new RecordingRepository();
var controller = new RegistrationController(repository);
var registration = new Registration("Ada", "ada@example.com", "Introduction");
var result = await controller.Register(registration, CancellationToken.None);

if (result is not CreatedResult || repository.Saved.Count != 1 || repository.Saved[0] != registration)
{
    Console.Error.WriteLine("VALIDATION FAILURE: baseline registration contract");
    return 1;
}

Console.WriteLine("dark factory starter contract: green");
return 0;

internal sealed class RecordingRepository : IRegistrationRepository
{
    public List<Registration> Saved { get; } = [];

    public Task SaveAsync(Registration registration, CancellationToken cancellationToken)
    {
        Saved.Add(registration);
        return Task.CompletedTask;
    }
}

