using CourseAPI.Datas;
using CourseAPI.Repositories;
using CourseAPI.Repositories.Interfaces;
using CourseAPI.Services;
using CourseAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using TechTrioCourses.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<CourseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CoursesContext")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Define policies BEFORE using them
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retryAttempt =>
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

//API url
builder.Services.AddHttpClient("UserAPI", client =>
{
    var config = builder.Configuration;
    var baseUrl = config["ApiSettings:UserAPI"];
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(circuitBreakerPolicy);

builder.Services.AddHttpClient("CategoryAPI", client =>
{
    var config = builder.Configuration;
    var baseUrl = config["ApiSettings:CategoryAPI"];
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(circuitBreakerPolicy);

builder.Services.AddHttpClient("LessonAPI", client =>
{
    var config = builder.Configuration;
    var baseUrl = config["ApiSettings:LessonAPI"];
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(circuitBreakerPolicy);

builder.Services.AddHttpClient("QuizAPI", client =>
{
    var config = builder.Configuration;
    var baseUrl = config["ApiSettings:QuizAPI"];
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(circuitBreakerPolicy);

builder.Services.AddScoped<ICourseRepo, CourseRepo>();
builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

// Add Memory Cache
builder.Services.AddMemoryCache();

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
