using System.Text.Json;
using InterestApi.Controllers;
using InterestApi.Domain;
using InterestApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace InterestApi.Tests;

public sealed class RegistrationControllerTests : IDisposable
{
    private readonly string _filePath = Path.Combine(Path.GetTempPath(), $"registrations-{Guid.NewGuid():N}.txt");

    [Fact]
    public async Task PersistsAndReturnsThePolicyDecisionForEveryAttempt()
    {
        var repository = new FileRegistrationRepository(_filePath);
        var controller = new RegistrationController(repository, new RegistrationPolicy());

        var accepted = await controller.Register(new Registration
        {
            Name = "Ada",
            Email = "ada@example.com",
            Course = "Introducción",
            HasAcceptedTerms = true
        });
        var rejected = await controller.Register(new Registration
        {
            Name = "Ada",
            Email = "ada@example.com",
            Course = "Introducción",
            HasAcceptedTerms = false
        });

        Assert.Equal("Accepted", ReadStatus(accepted));
        Assert.Equal("Rejected", ReadStatus(rejected));
        Assert.Equal(
            new[] { RegistrationDecision.Accepted, RegistrationDecision.Rejected },
            (await repository.GetAllAsync()).Select(registration => registration.Status));
    }

    public void Dispose()
    {
        if (File.Exists(_filePath)) File.Delete(_filePath);
    }

    private static string ReadStatus(IActionResult result)
    {
        var created = Assert.IsType<CreatedResult>(result);
        using var document = JsonDocument.Parse(JsonSerializer.Serialize(created.Value));
        return document.RootElement.GetProperty("status").GetString()!;
    }
}
