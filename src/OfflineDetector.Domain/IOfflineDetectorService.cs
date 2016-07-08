using System.Threading.Tasks;
using OfflineDetector.Domain.Configuration;

namespace OfflineDetector.Domain
{
    public interface IOfflineDetectorService
    {
        Task Start(IConfiguration configuration);
    }
}
