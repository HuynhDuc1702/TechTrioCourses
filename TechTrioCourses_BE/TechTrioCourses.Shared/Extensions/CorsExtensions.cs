using Microsoft.Extensions.DependencyInjection;

namespace TechTrioCourses.Shared.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddTechTrioCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                    {
                        policy.WithOrigins(
                    "http://localhost:3000",
             "https://localhost:3000",
   "http://localhost:5173"
       )
      .AllowAnyHeader()
        .AllowAnyMethod()
          .AllowCredentials();
                    });
            });

            return services;
        }
    }
}
