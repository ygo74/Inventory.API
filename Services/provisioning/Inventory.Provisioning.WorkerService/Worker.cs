using Inventory.Common.Infrastructure.Events;

namespace Inventory.Provisioning.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEventBus _eventBus;

        public Worker(ILogger<Worker> logger, IEventBus eventBus)
        {
            _logger = logger;
            eventBus.Subscribe<ProvisioningEvent, ProvisioningEventHandler>("dctest");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            _logger.LogDebug("Worker is starting.");

            stoppingToken.Register(() => _logger.LogDebug("#1 Worker background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }


            _logger.LogDebug("Worker background task is stopping.");

        }
    }
}