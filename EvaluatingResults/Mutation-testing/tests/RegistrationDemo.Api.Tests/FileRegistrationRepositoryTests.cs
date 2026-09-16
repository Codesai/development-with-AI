using InterestApi.Domain;
using InterestApi.Repository;

namespace InterestApi.Tests;

public sealed class FileRegistrationRepositoryTests : IDisposable
{
    private readonly string _filePath = Path.Combine(Path.GetTempPath(), $"registrations-{Guid.NewGuid():N}.txt");

    [Fact]
    public async Task SavesAndReadsTheRegistrationStatus()
    {
        await File.WriteAllTextAsync(_filePath, "Timestamp\tName\tEmail\tCourse\tStatus\n");
        var repository = new FileRegistrationRepository(_filePath);

        await repository.SaveAsync(new Registration
        {
            Name = "Ada",
            Email = "ada@example.com",
            Course = "Introducción",
            Status = RegistrationDecision.Waitlisted
        });

        var registration = Assert.Single(await repository.GetAllAsync());
        Assert.Equal(RegistrationDecision.Waitlisted, registration.Status);
    }

    [Fact]
    public async Task ReadsLegacyFourColumnRowsAsAccepted()
    {
        await File.WriteAllTextAsync(
            _filePath,
            "Timestamp\tName\tEmail\tCourse\n2026-07-06T23:22:59.5425033Z\tAda\tada@example.com\tIntroducción\n");
        var repository = new FileRegistrationRepository(_filePath);

        var registration = Assert.Single(await repository.GetAllAsync());
        Assert.Equal(RegistrationDecision.Accepted, registration.Status);
    }

    public void Dispose()
    {
        if (File.Exists(_filePath)) File.Delete(_filePath);
    }
}
