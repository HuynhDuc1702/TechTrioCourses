using Microsoft.EntityFrameworkCore;
using QuizAPI.Datas;
using QuizAPI.Repositories;
using QuizAPI.Repositories.Interfaces;
using QuizAPI.Services;
using QuizAPI.Services.Interfaces;
using Polly;
using Polly.Extensions.Http;
using TechTrioCourses.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<QuizzesContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString("QuizzesContext")));

// Add services to the container.

// Define policies
var retryPolicy = HttpPolicyExtensions
  .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retryAttempt =>
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

//API url
builder.Services.AddHttpClient("LessonAPI", client =>
{
    var config = builder.Configuration;
    var baseUrl = config["ApiSettings:LessonAPI"];
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(circuitBreakerPolicy);

// Register repositories
builder.Services.AddScoped<IQuestionRepo, QuestionRepo>();
builder.Services.AddScoped<IQuizRepo, QuizRepo>();
builder.Services.AddScoped<IQuestionChoiceRepo, QuestionChoiceRepo>();
builder.Services.AddScoped<IQuestionAnswerRepo, QuestionAnswerRepo>();
builder.Services.AddScoped<IQuizQuestionRepo, QuizQuestionRepo>();

// Register services
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IQuestionChoiceService, QuestionChoiceService>();
builder.Services.AddScoped<IQuestionAnswerService, QuestionAnswerService>();
builder.Services.AddScoped<IQuizQuestionService, QuizQuestionService>();

builder.Services.AddAutoMapper(typeof(Program));

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
