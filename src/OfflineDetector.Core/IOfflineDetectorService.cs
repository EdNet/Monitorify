using System;
using OfflineDetector.Core.Configuration;

namespace OfflineDetector.Core
{
    public interface IOfflineDetectorService
    {
        event Action<EndPoint> ListenerStarted;
        event Action<EndPoint> ListenerEnded;
        void Start(IConfiguration configuration);
    }
}
