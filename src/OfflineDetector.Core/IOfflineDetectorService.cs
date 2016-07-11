using System;
using OfflineDetector.Core.Configuration;

namespace OfflineDetector.Core
{
    public interface IOfflineDetectorService
    {
        event Action<EndPoint> ListenerStarted;
        event Action<EndPoint> ListenerEnded;
        event Action<EndPoint> ReportedOffline;
        event Action<EndPoint> ReportedOnline;
        event Action<Exception> ErrorOccured;

        void Start(IConfiguration configuration);
    }
}
