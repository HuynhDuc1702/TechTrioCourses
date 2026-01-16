using TechTrioCourses.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add YARP reverse proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add shared CORS
builder.Services.AddTechTrioCors();

// Add shared JWT (optional at gateway level)
// builder.Services.AddTechTrioJwtAuthentication(builder.Configuration);

var app = builder.Build();

// Use CORS
app.UseCors("AllowFrontend");

// Map reverse proxy
app.MapReverseProxy();

app.Run();
