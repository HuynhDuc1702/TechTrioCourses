using Microsoft.EntityFrameworkCore;
using UserAPI.Datas;
using UserAPI.Repositories;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services;
using UserAPI.Services.Interfaces;
using Polly;
using Polly.Extensions.Http;
using TechTrioCourses.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TechTrioUsersContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TechTrioUsersContext")));

// Register repositories
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserCourseRepo, UserCourseRepo>();
builder.Services.AddScoped<IUserLessonRepo, UserLessonRepo>();
builder.Services.AddScoped<IUserQuizRepo, UserQuizRepo>();
builder.Services.AddScoped<IUserQuizzeResultRepo, UserQuizzeResultRepo>();
builder.Services.AddScoped<IUserInputAnswerRepo, UserInputAnswerRepo>();
builder.Services.AddScoped<IUserSelectedChoiceRepo, UserSelectedChoiceRepo>();

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserCourseService, UserCourseService>();
builder.Services.AddScoped<IUserLessonService, UserLessonService>();
builder.Services.AddScoped<IUserQuizService, UserQuizService>();
builder.Services.AddScoped<IUserQuizzeResultService, UserQuizzeResultService>();
builder.Services.AddScoped<IUserInputAnswerService, UserInputAnswerService>();
builder.Services.AddScoped<IUserSelectedChoiceService, UserSelectedChoiceService>();
builder.Services.AddScoped<IUserCourseProgress, UserCourseProgress>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Add Memory Cache
builder.Services.AddMemoryCache();

// Define policies
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
  .WaitAndRetryAsync(3, retryAttempt =>
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

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

// Add shared CORS configuration
builder.Services.AddTechTrioCors();

// Configure shared JWT Authentication
builder.Services.AddTechTrioJwtAuthentication(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
