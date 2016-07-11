using System;
using OfflineDetector.Core.Configuration;

namespace OfflineDetector.Core
{
    public interface IOfflineDetectorService
    {
        event Action<EndPoint> ListenerStarted;
        void Start(IConfiguration configuration);
    }
}
