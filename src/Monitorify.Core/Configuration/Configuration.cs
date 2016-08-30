using System;
using System.Collections.Generic;

namespace Monitorify.Core.Configuration
{
    public class Configuration : IConfiguration
    {
        public IEnumerable<EndPoint> EndPoints { get; set; }
        public TimeSpan PingDelay { get; set; }
    }
}