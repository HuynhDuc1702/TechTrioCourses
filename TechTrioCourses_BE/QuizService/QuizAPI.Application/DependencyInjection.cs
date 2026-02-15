using Microsoft.Extensions.DependencyInjection;
using QuizAPI.Application.Interfaces.IServices;
using QuizAPI.Application.Services;
using System.Reflection;


namespace QuizAPI.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IQuestionChoiceService, QuestionChoiceService>();
            services.AddScoped<IQuestionAnswerService, QuestionAnswerService>();
            services.AddScoped<IQuizQuestionService, QuizQuestionService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMemoryCache();
            return services;
        }
    }
}
