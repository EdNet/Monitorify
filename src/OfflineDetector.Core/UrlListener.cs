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
            this._endPoint.Delay = GenerateDelay();
        }

        private int GenerateDelay()
        {
            Random rnd = new Random();
            return rnd.Next(1, 5) * 1000;
        }

        public Task StartListening()
        {
            ListenerStarted?.Invoke(_endPoint);
            return Task.Delay(_endPoint.Delay)
                .ContinueWith((prevTask) =>
            {
                ListenerEnded?.Invoke(_endPoint);
            });
        }
    }
}