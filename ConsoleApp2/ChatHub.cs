using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace ConsoleApp1
{
    public class ChatHub : Hub
    {

        private static readonly ConcurrentDictionary<string, string> Connections = new ConcurrentDictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Request.Query["userId"];

            Connections.TryAdd(Context.ConnectionId, Context.ConnectionId);
            await base.OnConnectedAsync();
            Console.WriteLine($"Connection established: {Context.ConnectionId}");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Connections.TryRemove(Context.ConnectionId, out _);
            await base.OnDisconnectedAsync(exception);
            Console.WriteLine($"Connection disconnected: {Context.ConnectionId}");
        }

    }
}
