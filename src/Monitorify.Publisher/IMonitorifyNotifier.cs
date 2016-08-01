using System;
using System.Threading.Tasks;
using Monitorify.Core.Configuration;

namespace Monitorify.Publisher
{
    public interface IMonitorifyNotifier
    {
        event Action<Exception> ErrorOccured;
        void AddPublisher(INotificationPublisher publisher);
        Task ListenAndNotify(IConfiguration configuration);
    }
}