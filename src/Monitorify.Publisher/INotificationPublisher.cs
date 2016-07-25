using System.Threading.Tasks;

namespace Monitorify.Publisher
{
    public interface INotificationPublisher
    {
        Task Notify();
    }
}
