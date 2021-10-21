using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PushNotifications.Service.Hubs;
using System.Threading;
using System.Threading.Tasks;

namespace PushNotifications.Service.Events
{
    public class PushNotificationHandler: INotificationHandler<PushNotificationEvent>
    {
        private readonly ILogger<PushNotificationHandler> _logger;
        private readonly IHubContext<PushNotificationHub> _hubContext;

        public PushNotificationHandler(ILogger<PushNotificationHandler> logger, IHubContext<PushNotificationHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task Handle(PushNotificationEvent @event, CancellationToken cancellationToken)
        {
            var notification = JsonConvert.SerializeObject(@event, new JsonSerializerSettings() 
            { 
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification, cancellationToken);

            _logger.LogInformation($"Send Notification!");
        }
    }
}
