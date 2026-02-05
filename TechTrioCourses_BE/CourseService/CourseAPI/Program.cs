
using CourseAPI.Infrastructure;
using CourseAPI.Application;
using Microsoft.EntityFrameworkCore;
using TechTrioCourses.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();





builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));


// Add shared CORS configuration
builder.Services.AddTechTrioCors();

// Configure shared JWT Authentication
builder.Services.AddTechTrioJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
