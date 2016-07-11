using System;
using System.Threading.Tasks;

namespace OfflineDetector.Core
{
    public interface IUrlListener
    {
        event Action<EndPoint> ListenerStarted;
        Task StartListening();
    }
}
