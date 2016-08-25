using System;
using Microsoft.Extensions.Configuration;
using Monitorify.Core;
using Xunit;

namespace Monitorify.Publisher.Email.Tests.Integration
{
    public class EmailNotificationPublisherTests
    {
        [Trait("Category", "Integration")]
        [Fact]
        public async void SendMessage_SmtpIsConfiguredCorrectly_MessageIsSent()
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
