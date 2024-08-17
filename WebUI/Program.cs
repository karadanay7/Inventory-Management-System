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



builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
        await accountService.SetUpAsync();
    }
}
app.MapSignOutEndpoint();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
