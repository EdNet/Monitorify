using System;
using Monitorify.Core.Configuration;

namespace Monitorify.Core
{
    public interface IMonitorifyService
    {
        event Action<EndPoint> ListenerStarted;
        event Action<EndPoint> ListenerEnded;
        event Action<EndPoint> ReportedOffline;
        event Action<EndPoint> ReportedOnline;
        event Action<Exception> ErrorOccured;

        void Start(IConfiguration configuration);
    }
}
