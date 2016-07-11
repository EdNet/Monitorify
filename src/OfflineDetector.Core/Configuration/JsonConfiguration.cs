using System.Collections.Generic;

namespace OfflineDetector.Domain.Configuration
{
    internal class JsonConfiguration : IConfiguration
    {
        public IEnumerable<EndPoint> EndPoints { get; set; }
    }
}