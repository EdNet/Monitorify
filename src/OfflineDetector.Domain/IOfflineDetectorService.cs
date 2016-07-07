using System.Collections.Generic;
using System.Threading.Tasks;

namespace OfflineDetector.Domain
{
    public interface IOfflineDetectorService
    {
        Task Start(IEnumerable<EndPoint> endpoints);
    }
}
