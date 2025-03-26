using CleanArchitecture.Rabbit;
using CleanArchitecture.WebSocket;
using CleanArchitecture.WebSocket.Authentications;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.ConfigureServices((context, services) =>
        {
            services.AddSignalR();

            services.AddSingleton<IMessageHandlerBase, AnotherMyMessageHandler>();
            services.AddSingleton<IMessageHandlerBase, MyMessageHandler>();

            services.AddRabbitReceiver();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost8080",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:8080")
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                    });
            });


            services.AddAuthentications();

            //services.AddAuthorization(options =>
            //{
            //    options.FallbackPolicy = options.DefaultPolicy;
            //});

        });

        webBuilder.Configure(app =>
        {
            
            app.UseRouting();

            app.UseCors("AllowLocalhost8080");

            app.UseAuthentication();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chatHub").RequireCors("AllowLocalhost8080");

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("SignalR server is running.");
                });
            });
        });
    })
    .ConfigureServices((context, services) =>
    {
        //services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

public class Worker : BackgroundService
{
    private readonly Receiver _receiver;

    public Worker(Receiver receiver/*, Sender sender*/)
    {
        _receiver = receiver;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        {

            // Subscribe to the queue
            await _receiver.Receive("hello");
        }
    }
}


