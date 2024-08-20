using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebUI.Hubs;

public class NetcodeHubConnectionService
{
    private readonly HubConnection _hubConnection;
    public NetcodeHubConnectionService(NavigationManager navigationManager)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri("/communicationhub"))
            .Build();
            _hubConnection.StartAsync();
            Console.WriteLine("Hub connection started");
    }
    public HubConnection GetHubConnection() => _hubConnection;
   

}
