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
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("configlibrary.settings.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddScoped<ITokenService, TokenService>();
            services.AddSingleton<ITokenValidationHelper, TokenValidationHelper>();

            return services;
        }
    }
}
