using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monitorify.Core;
using Monitorify.Core.Configuration;

namespace Monitorify.Publisher
{
    public class MonitorifyNotifier : IMonitorifyNotifier
    {
        private readonly List<INotificationPublisher> _publishers;
        private Func<IMonitorifyService> _monitorifyServiceFunc;

        public MonitorifyNotifier()
        {
            _publishers = new List<INotificationPublisher>();
        }

        public void AddPublisher(INotificationPublisher publisher)
        {
            _publishers.Add(publisher);
        }

        public Task ListenAndNotify(IConfiguration configuration)
        {
            var monitorifyService = _monitorifyServiceFunc.Invoke();
            monitorifyService.WentOffline += MonitorifyServiceOnWentOffline;
            monitorifyService.BackOnline += MonitorifyServiceOnBackOnline;

            return monitorifyService.Start(configuration);
        }

        internal Func<IMonitorifyService> MonitorifyServiceFactory
        {
            get { return _monitorifyServiceFunc ?? (_monitorifyServiceFunc = () => new MonitorifyService()); }
            set { _monitorifyServiceFunc = value; }
        }

        private void MonitorifyServiceOnBackOnline(EndPoint endPoint)
        {
            if (_publishers.Count > 0)
            {
                _publishers.ForEach(x => x.NotifyBackOnline(endPoint).Start());
            }
        }

        private void MonitorifyServiceOnWentOffline(EndPoint endPoint)
        {
            if (_publishers.Count > 0)
            {
                _publishers.ForEach(x => x.NotifyOffline(endPoint).Start());
            }
        }
    }
}