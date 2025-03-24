using CleanArchitecture.Rabbit;
using ConsoleApp1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.ConfigureServices((context, services) =>
        {
            services.AddSignalR();

            services.AddSingleton<IMessageHandlerBase, AnotherMyMessageHandler>();
            services.AddSingleton<IMessageHandlerBase, MyMessageHandler>();

            services.AddRabbitReceiver();
            services.AddRabbitSender();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials()
                 .SetIsOriginAllowed((host) => true));
            });

        });

        webBuilder.Configure(app =>
        {
            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chatHub");

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("SignalR server is running.");
                });
            });
        });
    })
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

public class Worker : BackgroundService
{
    private readonly Receiver _receiver;
    private readonly Sender _sender;

    public Worker(Receiver receiver, Sender sender)
    {
        _receiver = receiver;
        _sender = sender;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //while (true)
        {

            // Subscribe to the queue
            await _receiver.Receive("hello");

            //// Send a message
            //for (int i = 0; i < 10; i++)
            //{
            //    await Task.Delay(3000);
            //    await _sender.Send($"Message {i}", "hello");
            //}

            //await _receiver.Receive("hi");

            //for (int i = 0; i < 10; i++)
            //{
            //    await Task.Delay(3000);
            //    await _sender.Send($"Message {i}", "hi");
            //}
        }
    }
}


