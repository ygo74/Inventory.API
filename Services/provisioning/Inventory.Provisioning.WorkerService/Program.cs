using Inventory.Common.Infrastructure.Events.RabbitMQ;
using Inventory.Provisioning.WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {

        services.AddSingleton<ProvisioningEventHandler>();

        services.AddRabbitMQService(context.Configuration);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
