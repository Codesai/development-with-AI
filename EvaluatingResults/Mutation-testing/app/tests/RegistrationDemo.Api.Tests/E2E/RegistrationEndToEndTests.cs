using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace InterestApi.Tests.E2E;

public sealed class RegistrationEndToEndTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    // The application writes interests.txt in the process working directory.
    private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "interests.txt");
    private readonly WebApplicationFactory<Program> _factory;

    public RegistrationEndToEndTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        if (File.Exists(_filePath)) File.Delete(_filePath);
    }

    [Fact]
    public async Task RegistersAUserAndWritesTheRegistrationToInterestsFile()
    {
        using var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/register", new
        {
            name = "Ada",
            email = "ada@example.com",
            course = "Introducción",
            hasAcceptedTerms = true
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.Equal("Accepted", body!["status"]);

        var line = Assert.Single(await File.ReadAllLinesAsync(_filePath));
        Assert.Equal(new[] { "Ada", "ada@example.com", "Introducción", "Accepted" }, line.Split('\t').Skip(1));
    }

    [Fact]
    public async Task RejectsTheRegistrationWhenTermsAreNotAccepted()
    {
        using var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/register", new
        {
            name = "Ada",
            email = "ada@example.com",
            course = "Introducción",
            hasAcceptedTerms = false
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.Equal("Rejected", body!["status"]);

        var line = Assert.Single(await File.ReadAllLinesAsync(_filePath));
        Assert.Equal(new[] { "Ada", "ada@example.com", "Introducción", "Rejected" }, line.Split('\t').Skip(1));
    }

    public void Dispose()
    {
        if (File.Exists(_filePath)) File.Delete(_filePath);
    }
}
