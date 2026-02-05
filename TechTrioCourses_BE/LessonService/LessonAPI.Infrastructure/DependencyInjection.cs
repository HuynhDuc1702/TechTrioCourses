
using LessonAPI.Application.Interfaces;
using LessonAPI.Application.Interfaces.IExternalServices;
using LessonAPI.Infrastructure.Data;
using LessonAPI.Infrastructure.ExternalServices;
using LessonAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;


namespace LessonAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<LessonDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("LessonContext")));

            services.AddScoped<ILessonRepository, LessonRepository>();

            services.AddScoped<ICourseApiClient, CourseApiClient>();
            

            // HTTP Clients with Polly
            var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
             TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            var circuitBreakerPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));


            services.AddHttpClient("CourseAPI", client =>
            {
                client.BaseAddress = new Uri(configuration["ApiSettings:CourseAPI"]);
                client.Timeout = TimeSpan.FromSeconds(10);
            }).AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy);


            return services;
        }
    }
}
