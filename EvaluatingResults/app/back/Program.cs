using InterestApi.Controllers;
using InterestApi.Domain;
using InterestApi.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddSingleton<FileRegistrationRepository>();
builder.Services.AddSingleton<RegistrationPolicy>();

var app = builder.Build();
app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

// Serve static files from wwwroot (front) - index.html will be served at /
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
