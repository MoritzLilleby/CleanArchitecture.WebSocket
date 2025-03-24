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
    public class MyMessageHandler : IMessageHandlerBase
    {
        private readonly ILogger<MyMessageHandler> logger;
        private readonly IHubContext<ChatHub> hubContext;

        public MyMessageHandler(ILogger<MyMessageHandler> logger, IHubContext<ChatHub> hubContext)
        {
            this.logger=logger;
            this.hubContext=hubContext;
        }

        [ReceivedMessageHandler("description", "hi")]
        public async Task HandleMessageAsync(string message)
        {
            this.logger.LogInformation($" [x] Received {message} from queue hi");

            await hubContext.Clients.All.SendAsync("ReceiveMessage", "You have received a message HandleMessageAsync");

            // Process the message here
            await Task.CompletedTask;
        }

        //[ReceivedMessageHandler("description", "hello")]
        //public async Task HandleMessageAsync2(string message)
        //{
        //    this.logger.LogInformation($" [x] Received {message} from queue hello");

        //    await hubContext.Clients.All.SendAsync("ReceiveMessage", "You have received a message HandleMessageAsync1");

        //    var chatHub = (IHubContext<ChatHub>)_serviceProvider.GetService(typeof(IHubContext<ChatHub>));

        //    // Send message to all users in SignalR
        //    await chatHub.Clients.All.SendAsync("ReceiveMessage", "You have received a message HandleMessageAsync11");


        //    // Process the message here
        //    await Task.CompletedTask;
        //}
    }
}
