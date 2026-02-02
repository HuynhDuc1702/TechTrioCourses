using CategoryAPI.Application.Interfaces;
using CategoryAPI.Infrastructure.Data;
using CategoryAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategoryAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<CategoryDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("CategoryContext")));

            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }
    }
}
