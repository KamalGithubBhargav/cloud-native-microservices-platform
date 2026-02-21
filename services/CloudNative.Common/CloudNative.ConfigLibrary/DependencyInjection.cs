using CloudNative.ConfigLibrary.Implementation;
using CloudNative.ConfigLibrary.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudNative.ConfigLibrary
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfigLibaray(
            this IServiceCollection services)
        {

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // runtime path
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // 2️⃣ Register IConfiguration in DI so other services can consume it
            services.AddSingleton<IConfiguration>(configuration);

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITokenValidationHelper, TokenValidationHelper>();
        
            return services;
        }
    }
}
