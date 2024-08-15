using System;
using Application.Extension.Identity;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class ServiceContainer
{
 public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
 {
     services.AddDbContext<AppDbContext>(o => o.UseSqlServer(config.GetConnectionString("Default")), ServiceLifetime.Scoped);
     services.AddAuthentication(options=> {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
     }).AddIdentityCookies();
     services.AddIdentityCore<ApplicationUser>()
     .AddEntityFrameworkStores<AppDbContext>()
     .AddSignInManager()
     .AddDefaultTokenProviders();

     services.AddAuthorizationBuilder().AddPolicy("AdministrationPolicy", adp =>
     {
        adp.RequireAuthenticatedUser();
        adp.RequireRole("Admin", "Manager");
     }).AddPolicy("UserPolicy", udp =>
     {
        udp.RequireAuthenticatedUser();
        udp.RequireRole("User");
     });
     return services;
    
    

    
}
}
