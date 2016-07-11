using OfflineDetector.Core.Configuration;

namespace OfflineDetector.Core
{
    public interface IOfflineDetectorService
    {
        void Start(IConfiguration configuration);
    }
}
