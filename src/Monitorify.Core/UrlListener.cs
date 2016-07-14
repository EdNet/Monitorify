using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Monitorify.Core
{
    internal class UrlListener : IUrlListener
    {
        private readonly EndPoint _endPoint;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public event Action<EndPoint> ListenerStarted;
        public event Action<EndPoint> ListenerEnded;
        public event Action<EndPoint> ReportedOffline;
        public event Action<EndPoint> ReportedOnline;
        public event Action<Exception> ErrorOccured;

        public UrlListener(EndPoint endPoint)
        {
            this._endPoint = endPoint;
        }

        public async Task StartListening(TimeSpan delay)
        {
            ListenerStarted?.Invoke(_endPoint);

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                await DoRequest();
                await Task.Delay(delay);
            }
            
            ListenerEnded?.Invoke(_endPoint);
        }

        private async Task DoRequest()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(_endPoint.Url))
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