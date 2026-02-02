using AccountAPI.Datas;
using AccountAPI.Repositories;
using AccountAPI.Repositories.Interfaces;
using AccountAPI.Services;
using AccountAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using TechTrioCourses.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AccountContext")));
builder.Services.AddHttpClient("UserAPI", client =>
{
    var config = builder.Configuration;
    var baseUrl = config["ApiSettings:UserAPI"];
    client.BaseAddress = new Uri(baseUrl);
});
// Register repositories
builder.Services.AddScoped<IAccountRepo, AccountRepo>();

// Register services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Add HttpClient for calling UserAPI
builder.Services.AddHttpClient();

// Add shared CORS configuration
builder.Services.AddTechTrioCors();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure shared JWT Authentication
builder.Services.AddTechTrioJwtAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
