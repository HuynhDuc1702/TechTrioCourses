
using CourseAPI.Application.Interfaces;
using CourseAPI.Application.Interfaces.IExternalServices;
using CourseAPI.Infrastructure.Data;
using CourseAPI.Infrastructure.ExternalServices;
using CourseAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<CourseDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("CourseContext")));

            services.AddScoped<ICourseRepository, CourseRepository>();

            services.AddScoped<ICategoryApiClient, CategoryApiClient>();
            services.AddScoped<IUserApiClient, UserApiClient>();
            services.AddScoped<ILessonApiClient, LessonApiClient>();
            services.AddScoped<IQuizApiClient, QuizApiClient>();

            // HTTP Clients with Polly
            var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
             TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            var circuitBreakerPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));


            services.AddHttpClient("CategoryAPI", client =>
            {
                client.BaseAddress = new Uri(configuration["ApiSettings:CategoryAPI"]);
                client.Timeout = TimeSpan.FromSeconds(10);
            }).AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy);

            services.AddHttpClient("LessonAPI", client =>
            {
                client.BaseAddress = new Uri(configuration["ApiSettings:LessonAPI"]);
                client.Timeout = TimeSpan.FromSeconds(10);
            }).AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy);

            services.AddHttpClient("UserAPI", client =>
            {
                client.BaseAddress = new Uri(configuration["ApiSettings:UserAPI"]);
                client.Timeout = TimeSpan.FromSeconds(10);
            }).AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy);

            services.AddHttpClient("QuizAPI", client =>
            {
                client.BaseAddress = new Uri(configuration["ApiSettings:QuizAPI"]);
                client.Timeout = TimeSpan.FromSeconds(10);
            }).AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy);

            return services;
        }
    }
}
