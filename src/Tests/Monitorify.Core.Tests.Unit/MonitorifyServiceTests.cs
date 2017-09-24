using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monitorify.Core.Configuration;
using Moq;
using NUnit.Framework;

namespace Monitorify.Core.Tests.Unit
{
    [TestFixture]
    public class MonitorifyServiceTests
    {
        [Test]
        public async Task Start_ListeneresAreConfigured_ListenerStartListeningIsExecuted()
        {
            // Arrange
            Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
            var endpoint = new EndPoint {Name = "Test", Url = "http://test.test"};
            var delay = TimeSpan.Zero;
            configurationMock.Setup(x => x.EndPoints).Returns(new List<EndPoint> { endpoint });
            configurationMock.Setup(x => x.PingDelay).Returns(delay);
            MonitorifyService monitorifyService = new MonitorifyService();

            Mock<IUrlListener> urlListenerMock = new Mock<IUrlListener>();
            monitorifyService.UrlListenerFactory = () => urlListenerMock.Object;

            // Act
            await monitorifyService.Start(configurationMock.Object);
            await Task.Delay(100);

            // Assert
            urlListenerMock.Verify(x => x.StartListening(endpoint, delay));
        }

        [Test]
        public async Task Start_EndpointIsOnline_OnlineTimeIsRecorded()
        {
            // Arrange
            Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
            var endpoint = new EndPoint { Name = "Test", Url = "http://test.test" };
            var delay = TimeSpan.Zero;
            configurationMock.Setup(x => x.EndPoints).Returns(new List<EndPoint> { endpoint });
            configurationMock.Setup(x => x.PingDelay).Returns(delay);
            MonitorifyService monitorifyService = new MonitorifyService();

            Mock<IUrlListener> urlListenerMock = new Mock<IUrlListener>();
            monitorifyService.UrlListenerFactory = () => urlListenerMock.Object;

            // Act
            await monitorifyService.Start(configurationMock.Object);
            urlListenerMock.Raise(m => m.ReportedOnline += point => { }, endpoint);

            // Assert
            Assert.True(endpoint.LastOnline != null);
            Assert.True(endpoint.Status == EndpointStatus.Online);
        }

        [Test]
        public async Task Start_EndpointIsOffline_OfflineTimeIsRecorded()
        {
            // Arrange
            Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
            var endpoint = new EndPoint { Name = "Test", Url = "http://test.test" };
            var delay = TimeSpan.Zero;
            configurationMock.Setup(x => x.EndPoints).Returns(new List<EndPoint> { endpoint });
            configurationMock.Setup(x => x.PingDelay).Returns(delay);
            MonitorifyService monitorifyService = new MonitorifyService();

            Mock<IUrlListener> urlListenerMock = new Mock<IUrlListener>();
            monitorifyService.UrlListenerFactory = () => urlListenerMock.Object;

            // Act
            await monitorifyService.Start(configurationMock.Object);
            urlListenerMock.Raise(m => m.ReportedOffline += point => { }, endpoint);

            // Assert
            Assert.True(endpoint.LastOffline != null);
            Assert.True(endpoint.Status == EndpointStatus.Offline);
        }

        [Test]
        public async Task Start_EndpointWasOfflineAndNowOnline_BackOnlineEventIsTriggered()
        {
            // Arrange
            Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
            var endpoint = new EndPoint { Name = "Test", Url = "http://test.test" };
            var delay = TimeSpan.Zero;
            configurationMock.Setup(x => x.EndPoints).Returns(new List<EndPoint> { endpoint });
            configurationMock.Setup(x => x.PingDelay).Returns(delay);
            MonitorifyService monitorifyService = new MonitorifyService();

            Mock<IUrlListener> urlListenerMock = new Mock<IUrlListener>();
            monitorifyService.UrlListenerFactory = () => urlListenerMock.Object;

            bool backOnlineIsTriggered = false;
            monitorifyService.BackOnline += point => { backOnlineIsTriggered = true; };

            // Act
            await monitorifyService.Start(configurationMock.Object);
            urlListenerMock.Raise(m => m.ReportedOnline += point => { }, endpoint);
            urlListenerMock.Raise(m => m.ReportedOffline += point => { }, endpoint);
            urlListenerMock.Raise(m => m.ReportedOnline += point => { }, endpoint);

            // Assert
            Assert.True(backOnlineIsTriggered);
            Assert.True(endpoint.LastOutageTimeSpan != null);
        }
    }
}
