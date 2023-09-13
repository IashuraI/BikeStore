using BikeStore.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BikeStore.Infrastructure.EntityFramework
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<OrderService> ();

            return services;
        }
    }
}
