using System.IO;
using OfflineDetector.Core;
using OfflineDetector.Core.Configuration;

namespace OfflineDetector.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var currentDirectoryPath = Directory.GetCurrentDirectory();

            IConfigurationReader provider = new JsonFileConfigurationReader();
            provider.SetSource($"{currentDirectoryPath}\\urls.json");
            IConfiguration configuration = provider.Read();

            IOfflineDetectorService offlineDetectorService = new OfflineDetectorService();
            offlineDetectorService.ListenerStarted += endpoint => System.Console.WriteLine($"Url listener {endpoint.Name} is Started");
            offlineDetectorService.ListenerEnded += endpoint => System.Console.WriteLine($"Url listener {endpoint.Name} is Ended for Url {endpoint.Url}");
            offlineDetectorService.Start(configuration);

            System.Console.ReadLine();
        }
    }
}
