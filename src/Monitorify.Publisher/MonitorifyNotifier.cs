using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Monitorify.Core;
using Monitorify.Core.Configuration;

namespace Monitorify.Publisher
{
    public class MonitorifyNotifier : IMonitorifyNotifier
    {
        private readonly ILogger _logger;
        private readonly List<INotificationPublisher> _publishers;
        private Func<IMonitorifyService> _monitorifyServiceFunc;

        public MonitorifyNotifier()
        {
            _publishers = new List<INotificationPublisher>();
            _logger = new LoggerFactory().AddConsole().CreateLogger("Monitorify.Publisher.MonitorifyNotifier");
        }

        public MonitorifyNotifier(ILogger logger)
        {
            _publishers = new List<INotificationPublisher>();
            _logger = logger;
        }

        public void AddPublisher(INotificationPublisher publisher)
        {
            _publishers.Add(publisher);
        }

        public Task ListenAndNotify(IConfiguration configuration)
        {
            var monitorifyService = MonitorifyServiceFactory.Invoke();
            monitorifyService.WentOffline += MonitorifyServiceOnWentOffline;
            monitorifyService.BackOnline += MonitorifyServiceOnBackOnline;
            monitorifyService.Offline += point =>
            {
                _logger.LogInformation($"Endpoint {point.Url} offline");
            };
            monitorifyService.Online += point =>
            {
                _logger.LogInformation($"Endpoint {point.Url} online");
            };
            monitorifyService.ErrorOccured += exception =>
            {
                _logger.LogError(exception.Message, exception);
            };

            return monitorifyService.Start(configuration);
        }

        internal Func<IMonitorifyService> MonitorifyServiceFactory
        {
            get { return _monitorifyServiceFunc ?? (_monitorifyServiceFunc = () => new MonitorifyService()); }
            set { _monitorifyServiceFunc = value; }
        }

        private void MonitorifyServiceOnBackOnline(EndPoint endPoint)
        {
            _logger.LogInformation($"Endpoint {endPoint.Url} is back online, notifications will be sent");

            if (_publishers.Count > 0)
            {
                _publishers.ForEach(x => x.NotifyBackOnline(endPoint).Start());
                _logger.LogInformation($"Online notifications are sent for {endPoint.Url}");
            }
        }

        private void MonitorifyServiceOnWentOffline(EndPoint endPoint)
        {
            _logger.LogInformation($"Endpoint {endPoint.Url} went offline, notifications will be sent");

            if (_publishers.Count > 0)
            {
                _publishers.ForEach(x => x.NotifyOffline(endPoint).Start());
                _logger.LogInformation($"Offline notifications are sent for {endPoint.Url}");
            }
        }
    }
}