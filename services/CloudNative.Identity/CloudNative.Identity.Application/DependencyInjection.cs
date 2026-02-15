using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CloudNative.Identity.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(new[]
            {
                Assembly.GetExecutingAssembly(),
                //typeof(GetCountriesHandler).Assembly
            }));

            return services;
        }

    }
}
