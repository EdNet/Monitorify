using System.IO;
using Monitorify.Core;
using Monitorify.Core.Configuration;

namespace Monitorify.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var currentDirectoryPath = Directory.GetCurrentDirectory();

            IConfigurationReader provider = new JsonFileConfigurationReader();
            provider.SetSource($"{currentDirectoryPath}\\urls.json");
            IConfiguration configuration = provider.Read();

            IMonitorifyService service = new MonitorifyService();
            service.ListenerStarted += endpoint => System.Console.WriteLine($"Url listener {endpoint.Name} is Started");
            service.ReportedOnline += endpoint => System.Console.WriteLine($"Endpoint for {endpoint.Name} is Online");
            service.ReportedOffline += endpoint => System.Console.WriteLine($"Endpoint for {endpoint.Name} is Offline!!!");
            service.ErrorOccured += ex => System.Console.WriteLine($"Error occured!!! {ex.Message}");
            service.ListenerEnded += endpoint => System.Console.WriteLine($"Url listener {endpoint.Name} is Ended for Url {endpoint.Url}");
            service.Start(configuration);

            System.Console.ReadLine();
        }
    }
}
