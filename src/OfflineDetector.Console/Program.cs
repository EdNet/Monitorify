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
            offlineDetectorService.ListenerStarted += OfflineDetectorService_ListenerStarted;
            offlineDetectorService.Start(configuration);

            System.Console.ReadLine();
        }

        private static void OfflineDetectorService_ListenerStarted(EndPoint obj)
        {
            System.Console.WriteLine($"Url listener {obj.Name} is started with delay of {obj.Delay}");
        }
    }
}
