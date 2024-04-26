using Moq;
using Moq.Protected;
using System.Net;

namespace MSharp.TestLibrary;
public static class HttpTestingExtensions
{
    /// <summary>
    /// Extension method to setup Http Mocks. Usage:
    /// Setup: HttpFactoryTestExtensions.SetupHttpClient(HttpStatusCode.OK, out _clientHandler, out _httpClientFactory);
    /// Verifying Requests: _clientHandler.Protected().Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    /// </summary>
    /// <param name="statusCode">Status Code to Return With</param>
    /// <param name="delegatingHandler">Delegating Handler to Send Requests</param>
    /// <param name="httpClientFactory">Client Factory to Mock</param>
    public static void SetupHttpClient(HttpStatusCode statusCode, out Mock<DelegatingHandler> delegatingHandler, out Mock<IHttpClientFactory> httpClientFactory, HttpContent? httpContent = null)
    {
        delegatingHandler = new Mock<DelegatingHandler>();

        HttpResponseMessage httpResponseMessage = new HttpResponseMessage(statusCode);
        httpResponseMessage.Content = httpContent;

        delegatingHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage)
            .Verifiable();
        delegatingHandler.As<IDisposable>().Setup(s => s.Dispose());

        var httpClient = new HttpClient(delegatingHandler.Object);

        httpClientFactory = new Mock<IHttpClientFactory>(MockBehavior.Strict);
        httpClientFactory.Setup(cf => cf.CreateClient(It.IsAny<string>())).Returns(httpClient).Verifiable();
    }
}
