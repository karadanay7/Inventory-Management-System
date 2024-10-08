using Application.Interface.Identity;
using Application.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DataAccess;
using Infrastructure.Repository;
using Application.Extension.Identity;
using Application.Service.Products.Commands.Products;
using Infrastructure.Repository.Orders.Handlers;

namespace Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            // Register DbContext with the correct connection string
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Scoped);

            // Configure authentication and identity services
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddIdentityCookies();

            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            // Configure authorization policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministrationPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Admin", "Manager");
                });

                options.AddPolicy("UserPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("User");
                });
            });

            services.AddCascadingAuthenticationState();
            // Register custom services
            services.AddScoped<IAccount, Account>(); // Ensure this line is present
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateProductCommand).Assembly));
               services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetGenericOrdersCountHandler).Assembly));
            services.AddScoped<DataAccess.IDbContextFactory<AppDbContext>, DbContextFactory<AppDbContext>>();

            return services;
        }
    }
}
 