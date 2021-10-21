using MediatR;
using System;

namespace PushNotifications.Service.Events
{
    public class PushNotificationEvent : INotification
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
