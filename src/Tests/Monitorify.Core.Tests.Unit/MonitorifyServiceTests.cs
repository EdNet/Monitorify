using System;
using System.Collections.Generic;
using Monitorify.Core.Configuration;
using Moq;
using Xunit;

namespace Monitorify.Core.Tests.Unit
{
    public class MonitorifyServiceTests
    {
        [Fact]
        public void Start_ListeneresAreConfigured_ListenerStartListeningIsExecuted()
        {
            // Arrange
            Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(x => x.EndPoints).Returns(new List<EndPoint>
            {
                new EndPoint { Name = "Test", Url = "http://test.test" }
            });
            configurationMock.Setup(x => x.PingDelay).Returns(TimeSpan.Zero);
            MonitorifyService monitorifyService = new MonitorifyService();

            Mock<IUrlListener> urlListenerMock = new Mock<IUrlListener>();
            monitorifyService.UrlListenerFactory = () => urlListenerMock.Object;

            // Act
            monitorifyService.Start(configurationMock.Object);

            // Assert
            urlListenerMock.Verify(x => x.StartListening(It.IsAny<EndPoint>(), It.IsAny<TimeSpan>()));
        }
    }
}
