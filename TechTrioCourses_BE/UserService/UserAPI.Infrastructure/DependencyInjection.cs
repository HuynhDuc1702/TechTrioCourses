using UserAPI.Application.Interfaces.IRepositories;

using UserAPI.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

using UserAPI.Infrastructure.ExternalServices;
using UserAPI.Infrastructure.Repositories;
using UserAPI.Application.Interfaces.IExternalServices;

namespace UserAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<UserDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("UserContext")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserCourseRepository, UserCourseRepository>();
            services.AddScoped<IUserLessonRepository, UserLessonRepository>();
            services.AddScoped<IUserQuizRepository, UserQuizRepository>();
            services.AddScoped<IUserQuizzeResultRepository, UserQuizzeResultRepository>();
            services.AddScoped<IUserInputAnswerRepository, UserInputAnswerRepository>();
            services.AddScoped<IUserSelectedChoiceRepository, UserSelectedChoiceRepository>();
            services.AddScoped<IUserQuizzeResultQueryRepository, UserQuizzeResultQueryRepository>();


            services.AddScoped<ILessonApiClient, LessonApiClient>();
            services.AddScoped<IQuizApiClient, QuizApiClient>();

         
            var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
             TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            var circuitBreakerPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));


           

            services.AddHttpClient("LessonAPI", client =>
            {
                client.BaseAddress = new Uri(configuration["ApiSettings:LessonAPI"]);
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
