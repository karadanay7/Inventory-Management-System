using Infrastructure.DependencyInjection;
using WebUI.Components;
using Application.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Infrastructure.DataAccess;
using WebUI.Components.Layout.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Application.Service;
using WebUI.States.Administration;
using WebUI.States;
using WebUI.States.User;
using WebUI.Hubs;
using Syncfusion.Blazor;
using NetcodeHub.Packages.Components.DataGrid;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthStateProvider>();
builder.Services.AddSingleton<ChangePasswordState>();
builder.Services.AddScoped<UserActiveOrderCountState>();
builder.Services.AddScoped<AdminActiveOrderCountState>();
builder.Services.AddScoped<GenericHomeHeaderState>();
builder.Services.AddScoped<NetcodeHubConnectionService>();
builder.Services.AddScoped<ICustomAuthorizationService, CustomAuthorizationService>();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddVirtualizationService();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzQ4NjMzQDMxMzgyZTM0MmUzMGJk");
builder.Services.AddMudServices();
builder.Services.AddSignalR();





builder.Logging.AddConsole();



var app = builder.Build();

// Test database connection


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapSignOutEndpoint();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapHub<CommunicationHub>("/communicationhub");

app.Run();
