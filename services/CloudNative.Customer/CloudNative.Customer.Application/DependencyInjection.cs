using Microsoft.Extensions.DependencyInjection;

namespace CloudNative.Customer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(new[]
            //{
            //    Assembly.GetExecutingAssembly(),
            //    typeof(LoginCommandHandler).Assembly
            //}));

            return services;
        }

    }
}
