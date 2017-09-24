using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Monitorify.Core.HttpWrapper;
using Moq;
using NUnit.Framework;

namespace Monitorify.Core.Tests.Unit
{
    [TestFixture]
    public class UrlListenerTests
    {
        [Test]
        public void StartListening_EndpointIsValid_HttpRequestIsMade()
        {
            // Arrange
            Mock<IHttpClient> httpMock = new Mock<IHttpClient>();

            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            httpMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(responseMessage));

            EndPoint endPoint = new EndPoint();
            endPoint.Url = "http://www.google.com";
            endPoint.Name = "Google";

            IUrlListener listener = new UrlListener();

            // Act
            listener.HttpClient = httpMock.Object;
            listener.StartListening(endPoint, TimeSpan.FromSeconds(3));

            // Assert
            httpMock.Verify(x => x.GetAsync(endPoint.Url), Times.AtLeastOnce);
        }

        [Test]
        public async Task StartListening_EndpointIsOnline_OnlineEventIsRised()
        {
            // Arrange
            Mock<IHttpClient> httpMock = new Mock<IHttpClient>();

            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            httpMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(responseMessage));

            EndPoint endPoint = new EndPoint();
            endPoint.Url = "http://www.google.com";
            endPoint.Name = "Google";

            bool endpointIsOnline = false;
            IUrlListener listener = new UrlListener();
            listener.HttpClient = httpMock.Object;
            listener.ReportedOnline += point =>
            {
                endpointIsOnline = true;
                listener.StopListening();
            };

            // Act
            await listener.StartListening(endPoint, TimeSpan.FromSeconds(3));

            // Assert
            Assert.True(endpointIsOnline);
        }

        [Test]
        public async Task StartListening_EndpointIsOffline_OfflineEventIsRised()
        {
            // Arrange
            Mock<IHttpClient> httpMock = new Mock<IHttpClient>();

            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            httpMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(responseMessage));

            EndPoint endPoint = new EndPoint();
            endPoint.Url = "http://www.dodgyurl.com";
            endPoint.Name = "Google";

            bool endpointIsOffline = false;
            IUrlListener listener = new UrlListener();
            listener.HttpClient = httpMock.Object;
            listener.ReportedOffline += point =>
            {
                endpointIsOffline = true;
                listener.StopListening();
            };

            // Act
            await listener.StartListening(endPoint, TimeSpan.FromSeconds(3));

            // Assert
            Assert.True(endpointIsOffline);
        }

        [Test]
        public async Task StartListening_ExceptionIsThrown_ErrorEventIsRaised()
        {
            // Arrange
            Mock<IHttpClient> httpMock = new Mock<IHttpClient>();

            httpMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Throws<Exception>();

            EndPoint endPoint = new EndPoint();
            endPoint.Url = "http://www.google.com";
            endPoint.Name = "Google";

            bool errorIsRaised = false;
            bool listenerIsEndedRaised = false;
            IUrlListener listener = new UrlListener();
            listener.ErrorOccured += point =>
            {
                errorIsRaised = true;
                listener.StopListening();
            };
            listener.ListenerEnded += point => listenerIsEndedRaised = true;

            // Act
            listener.HttpClient = httpMock.Object;
            await listener.StartListening(endPoint, TimeSpan.FromSeconds(0));

            // Assert
            Assert.True(errorIsRaised);
            Assert.True(listenerIsEndedRaised);
        }
    }
}
