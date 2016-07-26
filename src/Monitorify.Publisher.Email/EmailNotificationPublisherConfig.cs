namespace Monitorify.Publisher.Email
{
    public class EmailNotificationPublisherConfig
    {
        public EmailNotificationPublisherConfig(string smtp)
        {
            SMTP = smtp;
        }

        public string SMTP { get; }
    }
}
