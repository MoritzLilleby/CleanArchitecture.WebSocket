using ConsoleApp1;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Rabbit
{
    public class AnotherMyMessageHandler : IMessageHandlerBase
    {
        private readonly ILogger<AnotherMyMessageHandler> logger;
        private readonly IHubContext<ChatHub> hubContext;

        public AnotherMyMessageHandler(ILogger<AnotherMyMessageHandler> logger, IHubContext<ChatHub> hubContext)
        {
            this.logger=logger;
            
            this.hubContext=hubContext;
        }

        [ReceivedMessageHandler("description", "hello")]
        public async Task HandleMessageAsync(string message)
        {
            logger.LogTrace($" [x] Received {message} on queue hello");
            // Process the message here

            // Send message to all users in SignalR
            await hubContext.Clients.All.SendAsync("ReceiveMessage", message);

            await Task.CompletedTask;
        }
    
    }
}
