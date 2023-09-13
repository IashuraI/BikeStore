using BikeStore.Application.Common.Jwt;
using BikeStore.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BikeStore.Infrastructure.EntityFramework
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<OrderService>();

            services.AddJWTTokenServices(configuration);

            return services;
        }

        public static IServiceCollection AddJWTTokenServices(this IServiceCollection services, IConfiguration Configuration)
        {
            // Add Jwt Setings
            var bindJwtSettings = new JwtSettings();
            Configuration.Bind("JsonWebTokenKeys", bindJwtSettings);

            services.AddSingleton(bindJwtSettings);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = bindJwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(bindJwtSettings.IssuerSigningKey)),
                    ValidateIssuer = bindJwtSettings.ValidateIssuer,
                    ValidIssuer = bindJwtSettings.ValidIssuer,
                    ValidateAudience = bindJwtSettings.ValidateAudience,
                    ValidAudience = bindJwtSettings.ValidAudience,
                    RequireExpirationTime = bindJwtSettings.RequireExpirationTime,
                    ValidateLifetime = bindJwtSettings.RequireExpirationTime,
                    ClockSkew = TimeSpan.FromDays(1),
                };
            });

            return services;
        }
    }
}
