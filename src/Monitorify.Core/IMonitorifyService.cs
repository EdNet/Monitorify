using System;
using Monitorify.Core.Configuration;

namespace Monitorify.Core
{
    public interface IMonitorifyService
    {
        event Action<EndPoint> ListenerStarted;
        event Action<EndPoint> ListenerEnded;
        event Action<EndPoint> Offline;
        event Action<EndPoint> Online;
        event Action<Exception> ErrorOccured;
        event Action<EndPoint> WentOffline;
        event Action<EndPoint> BackOnline;

        void Start(IConfiguration configuration);
    }
}
