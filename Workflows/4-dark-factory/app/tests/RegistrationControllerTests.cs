using InterestApi.Controllers;
using InterestApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace InterestApi.Tests;

public class RegistrationControllerTests
{
    [Fact]
    public async Task Register_WithARegistration_ReturnsCreated()
    {
        var controller = new RegistrationController();
        var registration = new Registration
        {
            Name = "Ada",
            Email = "ada@example.com",
            Course = "Introduction"
        };

        var result = await controller.Register(registration);

        var created = Assert.IsType<CreatedResult>(result);
        Assert.Equal("/api/register", created.Location);
    }
}
