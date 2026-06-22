using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

// Serve static files from wwwroot (front) - index.html will be served at /
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/api/register", async (HttpRequest req) =>
{
    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    Registration? registration;
    try
    {
        registration = await JsonSerializer.DeserializeAsync<Registration>(req.Body, options);
    }
    catch (JsonException)
    {
        return Results.BadRequest(new { error = "invalid json" });
    }

    if (registration is null || string.IsNullOrWhiteSpace(registration.Name) || string.IsNullOrWhiteSpace(registration.Email))
        return Results.BadRequest(new { error = "name and email required" });

    var line = $"{DateTime.UtcNow:O}\t{registration.Name}\t{registration.Email}\t{registration.Course}\n";
    var path = Path.Combine(Directory.GetCurrentDirectory(), "interests.txt");
    await File.AppendAllTextAsync(path, line);
    return Results.Created("/api/register", new { status = "ok" });
});

app.Run();

record Registration(string Name, string Email, string Course);
