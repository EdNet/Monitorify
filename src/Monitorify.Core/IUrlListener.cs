using System;
using System.Threading.Tasks;

namespace Monitorify.Core
{
    public interface IUrlListener
    {
        event Action<EndPoint> ListenerStarted;
        event Action<EndPoint> ListenerEnded;
        event Action<EndPoint> ReportedOffline;
        event Action<EndPoint> ReportedOnline;
        event Action<Exception> ErrorOccured;
        Task StartListening(TimeSpan delay);
    }
}
