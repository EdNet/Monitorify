using System.Threading.Tasks;

namespace OfflineDetector.Core
{
    class UrlListener : IUrlListener
    {
        private EndPoint _endPoint;

        public UrlListener(EndPoint endPoint)
        {
            this._endPoint = endPoint;
        }

        public Task StartListening()
        {
            throw new System.NotImplementedException();
        }
    }
}