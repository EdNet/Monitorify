using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Monitorify.Core.HttpWrapper;

namespace Monitorify.Core
{
    internal class UrlListener : IUrlListener
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private IHttpClient _httpClient;

        public event Action<EndPoint> ListenerStarted;
        public event Action<EndPoint> ListenerEnded;
        public event Action<EndPoint> ReportedOffline;
        public event Action<EndPoint> ReportedOnline;
        public event Action<Exception> ErrorOccured;

        public IHttpClient HttpClient
        {
            get { return _httpClient ?? (_httpClient = new HttpClientWrapper()); }
            set { _httpClient = value; }
        }

        public async Task StartListening(EndPoint endPoint, TimeSpan delay)
        {
            ListenerStarted?.Invoke(endPoint);

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                await DoRequest(endPoint);
                await Task.Delay(delay);
            }

            ListenerEnded?.Invoke(endPoint);
        }

        private async Task DoRequest(EndPoint endPoint)
        {
            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync(endPoint.Url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ReportedOnline?.Invoke(endPoint);
                    }
                    else
                    {
                        ReportedOffline?.Invoke(endPoint);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorOccured?.Invoke(ex);
                this._cancellationTokenSource.Cancel();
            }
        }
    }
}