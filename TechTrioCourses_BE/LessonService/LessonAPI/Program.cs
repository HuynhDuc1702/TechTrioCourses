using LessonAPI.Datas;
using LessonAPI.Repositories;
using LessonAPI.Repositories.Interfaces;
using LessonAPI.Services;
using LessonAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using TechTrioCourses.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<LessonDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("LessonContext")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Define policies
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retryAttempt =>
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

//API url
builder.Services.AddHttpClient("CourseAPI", client =>
{
    var config = builder.Configuration;
    var baseUrl = config["ApiSettings:CourseAPI"];
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(circuitBreakerPolicy);

builder.Services.AddScoped<ILessonRepo, LessonRepo>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddAutoMapper(typeof(Program));

// Add shared CORS configuration
builder.Services.AddTechTrioCors();

// Configure shared JWT Authentication
builder.Services.AddTechTrioJwtAuthentication(builder.Configuration);

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
