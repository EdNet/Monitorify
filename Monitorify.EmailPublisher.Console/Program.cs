using System;
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

            EmailNotificationPublisherConfig emailConfig = new EmailNotificationPublisherConfig("smtp.live.com", 25, true, "monitorify@outlook.com",
                "*******", "monitorify@outlook.com", "eduard.truuvaart@hotmail.com");

            IMonitorifyNotifier notifier = new MonitorifyNotifier();
            notifier.AddEmailPublisher(emailConfig);
            notifier.ErrorOccured += exception => { System.Console.WriteLine($"Error occured!!! {exception.Message}"); };
            notifier.ListenAndNotify(configuration).Wait();

            System.Console.ReadLine();
        }
    }
}
