using System;
using System.Threading.Tasks;

namespace Monitorify.Publisher.Email
{
    public class EmailNotificationPublisher : INotificationPublisher
    {
        private readonly EmailNotificationPublisherConfig _publisherConfig;

        public EmailNotificationPublisher(EmailNotificationPublisherConfig publisherConfig)
        {
            _publisherConfig = publisherConfig;
        }

        public Task Notify()
        {
            throw new NotImplementedException();
            /*            using (var client = new SmtpClient())
                        {
                            client.Connect("smtp.friends.com", 587, false);

                            // Note: since we don't have an OAuth2 token, disable
                            // the XOAUTH2 authentication mechanism.
                            client.AuthenticationMechanisms.Remove("XOAUTH2");

                            // Note: only needed if the SMTP server requires authentication
                            client.Authenticate("joey", "password");

                            client.Send(message);
                            client.Disconnect(true);
                        }*/
        }
    }
}