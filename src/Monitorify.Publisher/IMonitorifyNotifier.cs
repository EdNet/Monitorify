using System;
using System.Threading.Tasks;
using Monitorify.Core.Configuration;

namespace Monitorify.Publisher
{
    public interface IMonitorifyNotifier
    {
        void AddPublisher(INotificationPublisher publisher);
        Task ListenAndNotify(IConfiguration configuration);
    }
}