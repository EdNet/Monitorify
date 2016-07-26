using System;

namespace Monitorify.Core
{
    public class EndPoint
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime? LastOnline { get; set; }
        public DateTime? LastOffline { get; set; }
    }
}
