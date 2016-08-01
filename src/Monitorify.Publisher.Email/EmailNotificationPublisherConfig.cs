namespace Monitorify.Publisher.Email
{
    public class EmailNotificationPublisherConfig
    {
        public EmailNotificationPublisherConfig(string smtpServer, int smtpPort,  bool useSsl,
            string userName, string password, string fromEmail, string toEmail)
        {
            SmtpServer = smtpServer;
            SmtpPort = smtpPort;
            UseSsl = useSsl;
            UserName = userName;
            Password = password;
            FromEmail = fromEmail;
            ToEmail = toEmail;
        }

        public string SmtpServer { get; }
        public int SmtpPort { get; }
        public bool UseSsl { get; }
        public string UserName { get; }
        public string Password { get; }
        public string FromEmail { get; }
        public string ToEmail { get; }
    }
}
