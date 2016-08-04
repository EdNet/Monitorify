using System;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using Monitorify.Core;

namespace Monitorify.Publisher.Email
{
    public class EmailNotificationPublisher : INotificationPublisher
    {
        private readonly EmailNotificationPublisherConfig _publisherConfig;
        private Func<IMailTransport> _mailTransportFactory;

        public EmailNotificationPublisher(EmailNotificationPublisherConfig publisherConfig)
        {
            _publisherConfig = publisherConfig;
        }

        public Task NotifyOffline(EndPoint endPoint)
        {
            return SendMessage("Endpoint went offline!", 
                $"Endpoint {endPoint.Url} is currently offline");
        }
        
        public Task NotifyBackOnline(EndPoint endPoint)
        {
            return SendMessage("Endpoint is back online!",
                $"Endpoint {endPoint.Url} is back online. Outage time span is {endPoint.LastOutageTimeSpan.Value.ToString(@"d\.hh\:mm\:ss")}");
        }

        public Func<IMailTransport> MailTransportFactory
        {
            get { return _mailTransportFactory ?? (_mailTransportFactory = () => new SmtpClient()); }
            set { _mailTransportFactory = value; }
        }

        private Task SendMessage(string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", _publisherConfig.FromEmail));
            message.To.Add(new MailboxAddress("", _publisherConfig.ToEmail));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = MailTransportFactory.Invoke())
            {
                client.Connect(_publisherConfig.SmtpServer, _publisherConfig.SmtpPort, _publisherConfig.UseSsl);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_publisherConfig.UserName, _publisherConfig.Password);

                return client.SendAsync(message).ContinueWith(task =>
                {
                    client.DisconnectAsync(true);
                });

            }
        }
    }
}