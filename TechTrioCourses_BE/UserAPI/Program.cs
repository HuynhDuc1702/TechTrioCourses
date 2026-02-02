using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrioCourses.Shared.Enums;
using TechTrioCourses.Shared.Extensions;
using UserAPI.Consumers;
using UserAPI.Datas;
using UserAPI.Repositories;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services;
using UserAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TechTrioUsersContext")));

// Register repositories
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserCourseRepo, UserCourseRepo>();
builder.Services.AddScoped<IUserLessonRepo, UserLessonRepo>();
builder.Services.AddScoped<IUserQuizRepo, UserQuizRepo>();
builder.Services.AddScoped<IUserQuizzeResultRepo, UserQuizzeResultRepo>();
builder.Services.AddScoped<IUserInputAnswerRepo, UserInputAnswerRepo>();
builder.Services.AddScoped<IUserSelectedChoiceRepo, UserSelectedChoiceRepo>();
builder.Services.AddScoped<IUserQuizzeResultQueryRepo, UserQuizzeResultQueryRepo>();

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
builder.Services.AddHttpClient("LessonAPI", client =>
{
    var config = builder.Configuration;
    var baseUrl = config["ApiSettings:LessonAPI"];
    client.BaseAddress = new Uri(baseUrl);
});
builder.Services.AddHttpClient("QuizAPI", client =>
{
    var config = builder.Configuration;
    var baseUrl = config["ApiSettings:QuizAPI"];
    client.BaseAddress = new Uri(baseUrl);
});

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