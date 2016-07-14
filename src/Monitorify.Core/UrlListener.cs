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
        private readonly EndPoint _endPoint;

        public event Action<EndPoint> ListenerStarted;
        public event Action<EndPoint> ListenerEnded;
        public event Action<EndPoint> ReportedOffline;
        public event Action<EndPoint> ReportedOnline;
        public event Action<Exception> ErrorOccured;

        public UrlListener(EndPoint endPoint)
        {
            _endPoint = endPoint;
        }

        public async Task StartListening(IHttpClient httpClient, TimeSpan delay)
        {
            ListenerStarted?.Invoke(_endPoint);

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                await DoRequest(httpClient);
                await Task.Delay(delay);
            }
            
            ListenerEnded?.Invoke(_endPoint);
        }

        private async Task DoRequest(IHttpClient httpClient)
        {
            try
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(_endPoint.Url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ReportedOnline?.Invoke(_endPoint);
                    }
                    else
                    {
                        ReportedOffline?.Invoke(_endPoint);
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