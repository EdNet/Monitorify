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
        private Func<IHttpClient> _httpClientFactory;

        public event Action<EndPoint> ListenerStarted;
        public event Action<EndPoint> ListenerEnded;
        public event Action<EndPoint> Offline;
        public event Action<EndPoint> Online;
        public event Action<Exception> ErrorOccured;
        public event Action<EndPoint> WentOffline;
        public event Action<EndPoint> BackOnline;

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
                    using (IHttpClient httpClient = HttpClientFactory.Invoke())
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

        internal Func<IHttpClient> HttpClientFactory
        {
            get { return _httpClientFactory ?? (_httpClientFactory = () => new HttpClientWrapper()); }
            set { _httpClientFactory = value; }
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

            listener.ReportedOnline += endpoint =>
            {
                if (endpoint.Status == EndpointStatus.Offline)
                {
                    endpoint.RecordOutageTime();
                    endpoint.RecordOnline();
                    BackOnline?.Invoke(endpoint);
                }
                else
                {
                    endpoint.RecordOnline();
                }

                Online?.Invoke(endpoint);
            };

            listener.ReportedOffline += endpoint =>
            {
                var prevStatus = endpoint.Status;
                endpoint.RecordOffline();

                if (prevStatus == EndpointStatus.Online)
                {
                    WentOffline?.Invoke(endpoint);
                }
                
                Offline?.Invoke(endpoint);
            };
        }
    }
}