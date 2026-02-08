using AccountAPI.Application.Interfaces;
using AccountAPI.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AccountAPI.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMemoryCache();
            return services;
        }
    }
}
