using CloudNative.Identity.Core.Caching;
using CloudNative.Identity.Core.Constants;
using CloudNative.Identity.Core.Repositories.AuthServices;
using CloudNative.Identity.Infrastructure.Caching;
using CloudNative.Identity.Infrastructure.Persistence.Auth;
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

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();

            // Redis
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(
                    configuration[CachingConstant.RedisConnection] ?? CachingConstant.LocalRedis
                ));

            services.AddScoped<IRefreshTokenStore, RefreshTokenStore>();

            return services;
        }
    }
}




