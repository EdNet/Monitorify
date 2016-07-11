using System.Threading.Tasks;

namespace OfflineDetector.Core
{
    public interface IUrlListener
    {
        Task StartListening();
    }
}
