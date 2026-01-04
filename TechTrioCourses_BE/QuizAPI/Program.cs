using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuizAPI.Datas;
using QuizAPI.Repositories;
using QuizAPI.Repositories.Interfaces;
using QuizAPI.Services;
using QuizAPI.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<QuizzesContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("QuizzesContext")));

// Add services to the container.
//API url
builder.Services.AddHttpClient("LessonAPI", client =>
{
    var config = builder.Configuration;
    var baseUrl = config["ApiSettings:LessonAPI"];
    client.BaseAddress = new Uri(baseUrl);
});

// Register repositories
builder.Services.AddScoped<IQuestionRepo, QuestionRepo>();
builder.Services.AddScoped<IQuizRepo, QuizRepo>();
builder.Services.AddScoped<IQuestionChoiceRepo, QuestionChoiceRepo>();
builder.Services.AddScoped<IQuestionAnswerRepo, QuestionAnswerRepo>();

// Register services
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IQuestionChoiceService, QuestionChoiceService>();
builder.Services.AddScoped<IQuestionAnswerService, QuestionAnswerService>();

builder.Services.AddAutoMapper(typeof(Program));

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
