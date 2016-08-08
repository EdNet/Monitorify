using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Monitorify.Core;
using Monitorify.Core.Configuration;

namespace Monitorify.Publisher
{
    public class MonitorifyNotifier : IMonitorifyNotifier, IDisposable
    {
        private readonly ILogger _logger;
        private readonly List<INotificationPublisher> _publishers;
        private readonly IMonitorifyService _monitorifyService;

        public MonitorifyNotifier()
        {
            _publishers = new List<INotificationPublisher>();
            _logger = new LoggerFactory().AddConsole().CreateLogger("Monitorify.Publisher.MonitorifyNotifier");
            _monitorifyService = new MonitorifyService();
        }

        public MonitorifyNotifier(ILogger logger)
        {
            _publishers = new List<INotificationPublisher>();
            _monitorifyService = new MonitorifyService();
            _logger = logger;
        }

        public MonitorifyNotifier(ILogger logger, IMonitorifyService monitorifyService)
        {
            _monitorifyService = monitorifyService;
            _publishers = new List<INotificationPublisher>();
            _logger = logger;
        }

        public void AddPublisher(INotificationPublisher publisher)
        {
            _publishers.Add(publisher);
        }

        public async Task ListenAndNotify(IConfiguration configuration)
        {
            _monitorifyService.WentOffline += MonitorifyServiceOnWentOffline;
            _monitorifyService.BackOnline += MonitorifyServiceOnBackOnline;
            _monitorifyService.Offline += point =>
            {
                _logger.LogInformation($"Endpoint {point.Url} offline");
            };
            _monitorifyService.Online += point =>
            {
                _logger.LogInformation($"Endpoint {point.Url} online");
            };
            _monitorifyService.ErrorOccured += exception =>
            {
                _logger.LogError(exception.Message, exception);
            };

            await _monitorifyService.Start(configuration);
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

        public void Dispose()
        {
            _monitorifyService.WentOffline -= MonitorifyServiceOnWentOffline;
            _monitorifyService.BackOnline -= MonitorifyServiceOnBackOnline;
        }
    }
}