using System.Threading.Tasks;

namespace Monitorify.Publisher
{
    public interface INotificationPublisher
    {
        Task NotifyOffline(Core.EndPoint endPoint);
        Task NotifyBackOnline(Core.EndPoint endPoint);
    }
}
