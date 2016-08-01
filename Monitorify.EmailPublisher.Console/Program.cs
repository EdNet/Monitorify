using System.IO;
using Monitorify.Core.Configuration;
using Monitorify.Publisher;
using Monitorify.Publisher.Email;

namespace Monitorify.EmailPublisher.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string filepath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

            IConfigurationReader provider = new JsonFileConfigurationReader();
            provider.SetSource(filepath);
            IConfiguration configuration = provider.Read();

            EmailNotificationPublisherConfig emailConfig = new EmailNotificationPublisherConfig("smtp.live.com", 587, true, "username", 
                "password", "robot@hotmail.com", "user@hotmail.com");

            IMonitorifyNotifier notifier = new MonitorifyNotifier();
            notifier.AddEmailPublisher(emailConfig);
            notifier.ListenAndNotify(configuration);
        }
    }
}
