using CloudNative.Identity.Core.Caching;
using CloudNative.Identity.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CloudNative.Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // SQL
            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(
            //        configuration.GetConnectionString("Default")
            //    ));

            //services.AddScoped<IUserRepository, UserRepository>();

            // Redis
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(
                    configuration["Redis:ConnectionString"] ?? "localhost:6379"
                ));

            services.AddScoped<IRefreshTokenStore, RefreshTokenStore>();

            return services;
        }
    }
}




