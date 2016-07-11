using System.Collections.Generic;

namespace OfflineDetector.Core.Configuration
{
    public interface IConfiguration
    {
        IEnumerable<EndPoint> EndPoints { get; }
    }
}
