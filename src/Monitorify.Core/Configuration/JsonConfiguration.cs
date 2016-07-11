using System;
using System.Collections.Generic;

namespace Monitorify.Core.Configuration
{
    internal class JsonConfiguration : IConfiguration
    {
        public IEnumerable<EndPoint> EndPoints { get; set; }
        public TimeSpan PingDelay { get; set; }
    }
}