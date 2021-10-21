using System.Threading;
using System.Threading.Tasks;

namespace PushNotifications.Service.Services
{
    public interface IEventService
    {
        Task Subscribe(string topic, CancellationToken cancellationToken);
    }
}
