using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Monitorify.Core.HttpWrapper;
using Moq;
using Xunit;

namespace Monitorify.Core.Tests.Unit
{
    public class UrlListenerTests
    {
        [Fact]
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

            IUrlListener listener = new UrlListener(endPoint);

            // Act
            listener.StartListening(httpMock.Object, TimeSpan.FromSeconds(3));

            // Assert
            httpMock.Verify(x => x.GetAsync(endPoint.Url), Times.AtLeastOnce);
        }
    }
}
