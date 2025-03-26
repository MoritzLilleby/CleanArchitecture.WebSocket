using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace CleanArchitecture.WebSocket
{
    [Authorize]
    public class ChatHub : Hub
    {

        private static readonly ConcurrentDictionary<string, string> Connections = new ConcurrentDictionary<string, string>();

        public override async Task OnConnectedAsync()
        {

            var kk = Context.User?.Identity?.Name;

            var accessToken = Context.GetHttpContext()?.Request.Query["access_token"];

            var email = Context.User?.FindFirst(ClaimTypes.Email)?.Value!;

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
