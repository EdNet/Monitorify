namespace Monitorify.Publisher.Email
{
    public class EmailNotificationPublisherConfig
    {
        public EmailNotificationPublisherConfig(string smtp)
        {
            Smtp = smtp;
        }

        public string Smtp { get; }
    }
}
