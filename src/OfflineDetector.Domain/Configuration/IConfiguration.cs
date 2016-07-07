using System.Collections.Generic;

namespace OfflineDetector.Domain.Configuration
{
    public interface IConfiguration
    {
        IEnumerable<EndPoint> EndPoints { get; }
    }
}
