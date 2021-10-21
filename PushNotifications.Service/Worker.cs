using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PushNotifications.Service.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PushNotifications.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEventService _subscriber;

        public Worker(ILogger<Worker> logger, IEventService subscriber)
        {
            _logger = logger;
            _subscriber = subscriber;
        }
    
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                        await _subscriber.Subscribe("event-push-notification", stoppingToken)
                            .ConfigureAwait(false);

                        await Task.Delay(1000, stoppingToken).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
            }, stoppingToken).ConfigureAwait(false);
        }
    }
}
