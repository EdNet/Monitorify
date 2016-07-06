using System.Threading.Tasks;

namespace OfflineDetector.Domain
{
    public interface IOfflineDetectorService
    {
        Task Start(IOfflineDetectorSettings settings);
    }
}
