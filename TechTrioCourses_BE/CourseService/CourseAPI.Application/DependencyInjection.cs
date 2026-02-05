using CourseAPI.Application.Interfaces;
using CourseAPI.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CourseAPI.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICourseService, CourseService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMemoryCache();
            return services;
        }
    }
}
