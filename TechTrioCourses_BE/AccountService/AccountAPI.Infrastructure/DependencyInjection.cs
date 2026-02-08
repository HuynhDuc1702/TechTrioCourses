
using AccountAPI.Application.Interfaces;
using AccountAPI.Application.Interfaces.IExternalServices;
using AccountAPI.Infrastructure.Data;
using AccountAPI.Infrastructure.Repositories;
using AccountAPI.Infrastructure.ExternalServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;


namespace AccountAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AccountDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("AccountContext")));

            services.AddScoped<IAccountRepository, AccountRepository>();


            services.AddScoped<IUserApiClient, UserApiClient>();


            // HTTP Clients with Polly
            var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
             TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            var circuitBreakerPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));


            services.AddHttpClient("UserAPI", client =>
            {
                client.BaseAddress = new Uri(configuration["ApiSettings:UserAPI"]);
                client.Timeout = TimeSpan.FromSeconds(10);
            }).AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy);


            return services;
        }
    }
}
