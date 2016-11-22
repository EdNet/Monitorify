using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Monitorify.Core.Configuration;
using Monitorify.Publisher;
using Monitorify.Publisher.Email;
using IConfiguration = Monitorify.Core.Configuration.IConfiguration;

namespace Monitorify.EmailPublisher.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            var envConfig = builder.Build();
            string password = envConfig["monitorify_smtpPassword"];

            if (string.IsNullOrEmpty(password))
            {
                System.Console.WriteLine("Password env variabale is not set!");
                return;
            }

            string filepath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

            IConfigurationReader provider = new JsonFileConfigurationReader();
            provider.SetSource(filepath);
            IConfiguration configuration = provider.Read();

            EmailNotificationPublisherConfig emailConfig = new EmailNotificationPublisherConfig("smtp-mail.outlook.com", 587, false, "monitorify@outlook.com",
                password, "monitorify@outlook.com", "monitorify@outlook.com");

            IMonitorifyNotifier notifier = new MonitorifyNotifier();
            notifier.AddEmailPublisher(emailConfig);
            notifier.ListenAndNotify(configuration).Wait();

            while (true) {
                Thread.Sleep(5000);
            }
        }
    }
}
