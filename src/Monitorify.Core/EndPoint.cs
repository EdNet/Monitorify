using System;

namespace Monitorify.Core
{
    public class EndPoint
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime? LastOnline { get; private set; }
        public DateTime? LastOffline { get; private set; }
        public EndpointStatus Status { get; private set; } = EndpointStatus.Online;

        public void RecordOnline()
        {
            Status = EndpointStatus.Online;
            LastOnline = DateTime.UtcNow;
        }

        public void RecordOffline()
        {
            Status = EndpointStatus.Offline;
            LastOffline = DateTime.UtcNow;
        }
    }
}
