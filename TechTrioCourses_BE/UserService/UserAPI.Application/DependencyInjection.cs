using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserAPI.Application.Interfaces;
using UserAPI.Application.Interfaces.IServices;
using UserAPI.Application.Services;

namespace UserAPI.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserCourseService, UserCourseService>();
            services.AddScoped<IUserLessonService, UserLessonService>();
            services.AddScoped<IUserQuizService, UserQuizService>();
            services.AddScoped<IUserQuizzeResultService, UserQuizzeResultService>();
            services.AddScoped<IUserInputAnswerService, UserInputAnswerService>();
            services.AddScoped<IUserSelectedChoiceService, UserSelectedChoiceService>();
            services.AddScoped<IUserCourseProgressService, UserCourseProgressService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMemoryCache();
            return services;
        }
    }
}
