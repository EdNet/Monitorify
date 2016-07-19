using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monitorify.Core.Configuration;
using Monitorify.Core.HttpWrapper;

namespace Monitorify.Core
{
    public class MonitorifyService : IMonitorifyService
    {
        private readonly IList<IUrlListener> _listeners;
        private Func<IUrlListener> _urlListenerFactory;

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
                IUrlListener listener = this.UrlListenerFactory.Invoke();
                _listeners.Add(listener);

                SubscribeForEvents(listener);
                Task.Factory.StartNew(() =>
                {
                    using (IHttpClient httpClient = new HttpClientWrapper())
                    {
                        listener.HttpClient = httpClient;
                        listener.StartListening(endPoint, configuration.PingDelay);
                    }
                });
            }
        }

        internal Func<IUrlListener> UrlListenerFactory
        {
            get { return _urlListenerFactory ?? (_urlListenerFactory = () => new UrlListener()); }
            set { _urlListenerFactory = value; }
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