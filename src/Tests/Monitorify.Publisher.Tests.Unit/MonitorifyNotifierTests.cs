using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Monitorify.Core;
using Moq;
using NUnit.Framework;

namespace Monitorify.Publisher.Tests.Unit
{
    [TestFixture]
    public class MonitorifyNotifierTests
    {
        [Test]
        public async Task ListenAndNotify_MonitorifyServiceIsStarted()
        {
            // Arrange
            var configurationMock = new Mock<Core.Configuration.IConfiguration>();
            var monitorifyServiceMock = new Mock<IMonitorifyService>();
            var loggerMock = new Mock<ILogger>();
            var notifier = new MonitorifyNotifier(loggerMock.Object, monitorifyServiceMock.Object);

            // Act
            await notifier.ListenAndNotify(configurationMock.Object);

            // Assert
            monitorifyServiceMock.Verify(x => x.Start(configurationMock.Object), Times.Once);
        }


        [Test]
        public async Task ListenAndNotify_EndpointWentOffline_PublishersTriggerNotifyOffline()
        {
            // Arrange
            var endpoint = new EndPoint { Name = "Test", Url = "http://test.test" };
            var publisherMock = new Mock<INotificationPublisher>();
            publisherMock.Setup(x => x.NotifyOffline(It.IsAny<EndPoint>())).Returns(new Task(() => { }));

            var configurationMock = new Mock<Core.Configuration.IConfiguration>();
            var monitorifyServiceMock = new Mock<IMonitorifyService>();
            var loggerMock = new Mock<ILogger>();
            var notifier = new MonitorifyNotifier(loggerMock.Object, monitorifyServiceMock.Object);
            notifier.AddPublisher(publisherMock.Object);

            // Act
            await notifier.ListenAndNotify(configurationMock.Object);
            monitorifyServiceMock.Raise(m => m.WentOffline += point => { }, endpoint);

            // Assert
            publisherMock.Verify(x => x.NotifyOffline(endpoint));
        }

        [Test]
        public async Task ListenAndNotify_EndpointBackOnline_PublishersTriggerBackOnline()
        {
            // Arrange
            var endpoint = new EndPoint { Name = "Test", Url = "http://test.test" };
            var publisherMock = new Mock<INotificationPublisher>();
            publisherMock.Setup(x => x.NotifyBackOnline(It.IsAny<EndPoint>())).Returns(new Task(() => { }));

            var configurationMock = new Mock<Core.Configuration.IConfiguration>();
            var monitorifyServiceMock = new Mock<IMonitorifyService>();
            var loggerMock = new Mock<ILogger>();
            var notifier = new MonitorifyNotifier(loggerMock.Object, monitorifyServiceMock.Object);
            notifier.AddPublisher(publisherMock.Object);

            // Act
            await notifier.ListenAndNotify(configurationMock.Object);
            monitorifyServiceMock.Raise(m => m.BackOnline += point => { }, endpoint);

            // Assert
            publisherMock.Verify(x => x.NotifyBackOnline(endpoint));
        }
    }
}
