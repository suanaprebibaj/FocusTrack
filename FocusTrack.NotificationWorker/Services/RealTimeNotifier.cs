//using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.NotificationWorker.Services
{
    public class RealTimeNotifier
    {
        private readonly HubConnection _connection;

        public RealTimeNotifier()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://api:8080/hubs/notifications")
                .WithAutomaticReconnect()
                .Build();

            _ = _connection.StartAsync();
        }

        public Task PushToUserAsync(Guid userId, string message)
            => _connection.InvokeAsync("SendNotification", userId, message);
    }
}
