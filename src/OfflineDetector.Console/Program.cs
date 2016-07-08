using System.IO;
using OfflineDetector.Domain;
using OfflineDetector.Domain.Configuration;

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
            offlineDetectorService.Start(configuration);

            System.Console.ReadLine();
        }
    }
}
