using System.Threading.Tasks;

namespace Monitorify.Publisher
{
    public interface IMonitorifyNotifier
    {
        Task ListenAndNotify();
    }
}