using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Monitorify.Core;
using NUnit.Framework;

namespace Monitorify.Publisher.Email.Tests.Integration
{
    [TestFixture]
    public class EmailNotificationPublisherTests
    {
        [Category("Integration")]
        [Test]
        public async Task SendMessage_SmtpIsConfiguredCorrectly_MessageIsSent()
        {
            // Arrange
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            var envConfig = builder.Build();
            string password = envConfig["monitorify_smtpPassword"];

            var endpoint = new EndPoint { Name = "Google", Url = "https://www.google.com", LastOutageTimeSpan = DateTime.UtcNow.AddMinutes(-1) - DateTime.UtcNow.AddMinutes(-2) };
            var config = new EmailNotificationPublisherConfig("smtp-mail.outlook.com", 587, false, "monitorify@outlook.com", password, "monitorify@outlook.com", "monitorify@outlook.com");
            var publisher = new EmailNotificationPublisher(config);

            // Act / Assert
            await publisher.NotifyBackOnline(endpoint);
        }
    }
}
