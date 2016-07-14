using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Monitorify.Core.HttpWrapper
{
    internal interface IHttpClient : IDisposable
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);
    }
}
