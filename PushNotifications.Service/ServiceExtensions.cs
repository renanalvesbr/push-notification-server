using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using PushNotifications.Service.Services;

namespace PushNotifications.Service
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddEventService(this IServiceCollection services)
        {
            services.AddTransient<IEventService, EventService>();

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "push-notification-service",
                EnableAutoCommit = false,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoOffsetStore = false
            };

            var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();

            services.AddSingleton(consumerConfig);
            services.AddSingleton(consumer);

            return services;
        }
    }
}
