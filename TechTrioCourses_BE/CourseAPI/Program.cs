using CourseAPI.Datas;
using CourseAPI.Repositories;
using CourseAPI.Repositories.Interfaces;
using CourseAPI.Services;
using CourseAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<TechTrioCoursesContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CoursesContext")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//API url
builder.Services.AddHttpClient("UserAPI", client =>
{
    var config = builder.Configuration;
    var baseUrl = config["ApiSettings:UserAPI"];
    client.BaseAddress = new Uri(baseUrl);
});
builder.Services.AddHttpClient("CategoryAPI", client =>
{
    var config = builder.Configuration;
    var baseUrl = config["ApiSettings:CategoryAPI"];
    client.BaseAddress = new Uri(baseUrl);
});
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

builder.Services.AddScoped<ICourseRepo, CourseRepo>();
builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173") // Add your frontend URL
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

var app = builder.Build();

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
