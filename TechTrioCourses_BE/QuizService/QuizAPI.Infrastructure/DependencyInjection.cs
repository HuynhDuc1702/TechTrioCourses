using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizAPI.Application.Interfaces.IRepositories;
using QuizAPI.Infrastructure.Data;
using QuizAPI.Infrastructure.Repositories;



namespace QuizAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<QuizDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("QuizzesContext")));
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IQuestionChoiceRepository, QuestionChoiceRepository>();
            services.AddScoped<IQuestionAnswerRepository, QuestionAnswerRepository>();
            services.AddScoped<IQuizQuestionRepository, QuizQuestionRepository>();
            services.AddScoped<IQuizQueryRepository, QuizQueryRepository>();
            return services;
        }
    }
}
