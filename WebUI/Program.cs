using Infrastructure.DependencyInjection;
using WebUI.Components;
using Application.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Infrastructure.DataAccess;
using WebUI.Components.Layout.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Application.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthStateProvider>();
builder.Services.AddScoped<ICustomAuthorizationService, CustomAuthorizationService>();
builder.Logging.AddConsole();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Test database connection
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var canConnect = await dbContext.TestConnectionAsync();
    if (canConnect)
    {
        Console.WriteLine("Database connection successful.");
    }
    else
    {
        Console.WriteLine("Database connection failed.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapSignOutEndpoint();
app.Run();
