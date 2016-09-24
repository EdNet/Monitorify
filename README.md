# What is Monitorify?
----------------------------------------
Monitorify is a simple url listener that triggers notification once endpoint goes down. Monitorify.Publisher.Email uses email client to send notifications.

# How do I get started?
----------------------------------------
1. Install NuGet
2. Install monitorify email publisher

    PM> Install-Package Monitorify.Publisher.Email

3. Add bootstrapping code:


            IConfiguration configuration = new Configuration
            {
                PingDelay = TimeSpan.FromSeconds(1),
                EndPoints = new List<EndPoint>
                {
                    new EndPoint {
                        Name = "Google",
                        Url = "http://www.google.com"
                    }
                }
            };
            EmailNotificationPublisherConfig emailConfig = new EmailNotificationPublisherConfig("smtp-mail.outlook.com", 587, false, "smtp-username",
                "smtp-password", "from-email@outlook.com", "to-email@outlook.com");

            IMonitorifyNotifier notifier = new MonitorifyNotifier();
            notifier.AddEmailPublisher(emailConfig);
            notifier.ListenAndNotify(configuration).Wait();