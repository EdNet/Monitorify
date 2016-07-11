using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monitorify.Core.Configuration;

namespace Monitorify.Core
{
    public class MonitorifyService : IMonitorifyService
    {
        private readonly IList<IUrlListener> _listeners;

        public event Action<EndPoint> ListenerStarted;
        public event Action<EndPoint> ListenerEnded;
        public event Action<EndPoint> ReportedOffline;
        public event Action<EndPoint> ReportedOnline;
        public event Action<Exception> ErrorOccured;

        public MonitorifyService()
        {
            _listeners = new List<IUrlListener>();
        }

        public void Start(IConfiguration configuration)
        {
            foreach (var endPoint in configuration.EndPoints)
            {
                IUrlListener listener = new UrlListener(endPoint);
                _listeners.Add(listener);

                SubscribeForEvents(listener);

                Task.Factory.StartNew(() => listener.StartListening());
            }
        }

        private void SubscribeForEvents(IUrlListener listener)
        {
            if (ErrorOccured != null)
            {
                listener.ErrorOccured += ex => ErrorOccured(ex);
            }
            if (ListenerStarted != null)
            {
                listener.ListenerStarted += endpoint => ListenerStarted(endpoint);
            }
            if (ListenerEnded != null)
            {
                listener.ListenerEnded += endpoint => ListenerEnded(endpoint);
            }
            if (ReportedOnline != null)
            {
                listener.ReportedOnline += endpoint => ReportedOnline(endpoint);
            }
            if (ReportedOffline != null)
            {
                listener.ReportedOffline += endpoint => ReportedOffline(endpoint);
            }
        }
    }
}