// -----------------------------------------------------------------------------
// Program.cs
// -----------------------------------------------------------------------------
// Application entry point. This file wires up the ASP.NET Core host: it builds
// the dependency injection container, configures the HTTP request pipeline, and
// finally starts listening for requests.
//
// The app is intentionally tiny: it serves a static HTML form from wwwroot and
// exposes a single JSON endpoint (POST /api/register) that appends a line to a
// text file. There is no database and no authentication.
// -----------------------------------------------------------------------------

using InterestApi.Controllers;

// Create the host builder. WebApplication.CreateBuilder reads configuration from
// appsettings.json, environment variables and command line arguments, and sets
// up logging with sensible defaults.
var builder = WebApplication.CreateBuilder(args);

// --- Service registration (the DI container) ---------------------------------

// Register CORS services so we can relax the same-origin policy below. The
// frontend is served from the same origin in production, but keeping CORS open
// makes local experiments (e.g. calling the API from a separate dev server)
// painless.
builder.Services.AddCors();

// Register the MVC controller services so attribute-routed controllers such as
// RegistrationController are discovered and activated per request.
builder.Services.AddControllers();

// --- Build the app and configure the middleware pipeline ---------------------

// From here on we are composing the pipeline. Order matters: each call to Use*
// adds a middleware that runs in the order it was added.
var app = builder.Build();

// Allow any origin, header and method. This is deliberately permissive because
// the project is a throwaway training sample, not a production service.
app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

// Serve static files from wwwroot (the built frontend). UseDefaultFiles rewrites
// a request for "/" to "/index.html" so the form loads at the site root, and
// UseStaticFiles actually returns the file contents.
app.UseDefaultFiles();
app.UseStaticFiles();

// Map attribute-routed controller actions (e.g. POST /api/register) into the
// pipeline so incoming requests can reach them.
app.MapControllers();

// Start the Kestrel web server and block until the process is asked to stop.
app.Run();
