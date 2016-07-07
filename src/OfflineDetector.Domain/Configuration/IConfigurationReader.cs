using System.Collections.Generic;

namespace OfflineDetector.Domain.Configuration
{
    public interface IConfigurationReader
    {
        IEnumerable<EndPoint> Read();
    }
}
