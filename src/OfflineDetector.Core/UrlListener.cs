using System;
using System.Threading.Tasks;

namespace OfflineDetector.Core
{
    class UrlListener : IUrlListener
    {
        private readonly EndPoint _endPoint;

        public event Action<EndPoint> ListenerStarted;
        public event Action<EndPoint> ListenerEnded;

        public UrlListener(EndPoint endPoint)
        {
            this._endPoint = endPoint;
        }

        public Task StartListening()
        {
            return Task.Factory.StartNew(() =>
            {
                ListenerStarted?.Invoke(_endPoint);
                ListenerEnded?.Invoke(_endPoint);
            });
        }
    }
}