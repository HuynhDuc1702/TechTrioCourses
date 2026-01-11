using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserAPI.Datas;
using UserAPI.Repositories;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services;
using UserAPI.Services.Interfaces;

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

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000", "http://localhost:5173")
              .AllowAnyHeader()
        .AllowAnyMethod()
     .AllowCredentials();
    });
});

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"] ?? "DefaultKey"))
    };
});

builder.Services.AddAuthorization();

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