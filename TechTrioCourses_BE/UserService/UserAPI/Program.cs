using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrioCourses.Shared.Extensions;
using UserAPI.Application;
using UserAPI.Infrastructure;
using UserAPI.Consumers;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program));


// Add CORS using shared extension
builder.Services.AddTechTrioCors();

// Configure JWT Authentication using shared extension
builder.Services.AddTechTrioJwtAuthentication(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// UserAPI/Program.cs
builder.Services.AddMassTransit(x =>
{
    // Register consumers
    x.AddConsumer<QuizGradedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Retry configuration
        cfg.UseMessageRetry(r =>
        {
            r.Interval(3, TimeSpan.FromSeconds(5));
            
        });

        // Configure endpoints
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();