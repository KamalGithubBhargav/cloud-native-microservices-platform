using CloudNative.ConfigLibrary.Constants;
using CloudNative.Customer.Core.Interfaces;
using CloudNative.Customer.Infrastructure.Database;
using CloudNative.Customer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CloudNative.Customer.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // SQL
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("Default")
                ));

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

            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }
    }
}
