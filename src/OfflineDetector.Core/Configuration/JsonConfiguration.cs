using System.Collections.Generic;

namespace OfflineDetector.Core.Configuration
{
    internal class JsonConfiguration : IConfiguration
    {
        public IEnumerable<EndPoint> EndPoints { get; set; }
    }
}