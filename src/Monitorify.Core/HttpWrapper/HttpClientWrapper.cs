using System.Net.Http;
using System.Threading.Tasks;

namespace Monitorify.Core.HttpWrapper
{
    internal class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper()
        {
            this._httpClient = new HttpClient();
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return _httpClient.GetAsync(requestUri);
        }

        public void Dispose() { }
    }
}