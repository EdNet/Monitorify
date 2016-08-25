using System;

namespace Monitorify.Publisher.Email
{
    public static class PublisherExtensions
    {
        public static void AddEmailPublisher(this IMonitorifyNotifier notifier, EmailNotificationPublisherConfig config)
        {
            notifier.AddPublisher(new EmailNotificationPublisher(config));
        }
    }
}