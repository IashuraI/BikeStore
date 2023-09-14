using BikeStore.Infrastructure.EntityFramework;
using BikeStore.Infrastructure.EntityFramework.Data;
using BikeStore.Infrastructure.EntityFramework.Identity;
using BikeStore.Persistence.Data;
using BikeStore.WebApi.Infra;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme."
        });
        options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
    }
);

builder.Services.AddInfrastructureEntityFramework(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ViewCreateEditPolicyForCustomer", policy =>
    {
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new CustomersResourceRequirement());
    });
});

builder.Services.AddIdentity<User, Role>(
        options => {
            options.SignIn.RequireConfirmedAccount = false;
        }
        )
    .AddEntityFrameworkStores<BikeStoreDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    //options.Cookie.Expiration 

    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
});

builder.Services.AddSingleton<IAuthorizationHandler, ResourceAuthorizationHandler>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<UserClaimsService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        await scope.ServiceProvider.GetRequiredService<DatabaseSeeding>().Seed();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
