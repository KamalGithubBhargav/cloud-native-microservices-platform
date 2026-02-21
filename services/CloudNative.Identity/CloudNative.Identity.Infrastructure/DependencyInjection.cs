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
            var connectionString =
                 configuration[CachingConstant.RedisConnection]
                 ?? CachingConstant.LocalRedis;

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var options = ConfigurationOptions.Parse(connectionString);

                // Recommended production settings
                options.AbortOnConnectFail = false;
                options.ConnectRetry = 5;
                options.ReconnectRetryPolicy = new ExponentialRetry(5000);
                options.KeepAlive = 60;
                options.ConnectTimeout = 10000;
                options.SyncTimeout = 10000;

                return ConnectionMultiplexer.Connect(options);
            });
            services.AddScoped<IRefreshTokenStore, RefreshTokenStore>();

            return services;
        }
    }
}




