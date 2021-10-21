using Confluent.Kafka;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PushNotifications.Service.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PushNotifications.Service.Services
{
    public class EventService : IEventService
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly IMediator _mediator;

        public EventService(ConsumerConfig consumerConfig, IMediator mediator)
        {
            _consumerConfig = consumerConfig;
            _mediator = mediator;
        }

        public async Task Subscribe(string topic, CancellationToken cancellationToken)
        {
            using var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();

            consumer.Subscribe(topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await NextEvent(consumer, cancellationToken).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                consumer.Close();
            }
        }

        private async Task NextEvent(IConsumer<string, string> consumer, CancellationToken cancellationToken)
        {
            var message = consumer.Consume(cancellationToken);

            if (message.Message != null)
            {
                var @event = JsonConvert.DeserializeObject<PushNotificationEvent>(message?.Message.Value, new JsonSerializerSettings() 
                { 
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                if (@event == null)
                    return;

                await _mediator.Publish(@event, cancellationToken).ConfigureAwait(false);

                try
                {
                    consumer.StoreOffset(message);
                    consumer.Commit(message);
                }
                catch (KafkaException)
                {
                    
                }
            }
        }
    }
}
