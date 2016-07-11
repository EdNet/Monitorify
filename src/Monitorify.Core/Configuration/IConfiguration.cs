using System;
using System.Collections.Generic;

namespace Monitorify.Core.Configuration
{
    public interface IConfiguration
    {
        IEnumerable<EndPoint> EndPoints { get; }
        TimeSpan PingDelay { get; }
    }
}
