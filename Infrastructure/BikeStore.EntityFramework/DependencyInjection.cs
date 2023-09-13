﻿using BikeStore.Application.Interfaces;
using BikeStore.Infrastructure.EntityFramework.Repositories.Sales;
using BikeStore.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeStore.Infrastructure.EntityFramework
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BikeStoreDbContext>
                (options => options.UseSqlServer(configuration.GetConnectionString("BikeStoreDb")));

            services.AddTransient<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
