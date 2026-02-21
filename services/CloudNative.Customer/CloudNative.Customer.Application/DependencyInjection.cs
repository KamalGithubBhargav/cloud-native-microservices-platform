using CloudNative.Customer.Application.Features.Customer.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CloudNative.Customer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(new[]
            {
                Assembly.GetExecutingAssembly(),
                typeof(CreateCustomerCommandHandler).Assembly
            }));

            return services;
        }

    }
}
