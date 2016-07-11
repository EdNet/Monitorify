using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OfflineDetector.Core
{
    class UrlListener : IUrlListener
    {
        private readonly EndPoint _endPoint;

        public event Action<EndPoint> ListenerStarted;
        public event Action<EndPoint> ListenerEnded;
        public event Action<EndPoint> ReportedOffline;
        public event Action<EndPoint> ReportedOnline;
        public event Action<Exception> ErrorOccured;

        public UrlListener(EndPoint endPoint)
        {
            this._endPoint = endPoint;
        }

        public async Task StartListening()
        {
            ListenerStarted?.Invoke(_endPoint);

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
            }

            ListenerEnded?.Invoke(_endPoint);
        }
    }
}