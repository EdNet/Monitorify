using System.IO;
using System.Threading;
using Monitorify.Core;
using Monitorify.Core.Configuration;

namespace Monitorify.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string filepath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

            IConfigurationReader provider = new JsonFileConfigurationReader();
            provider.SetSource(filepath);
            IConfiguration configuration = provider.Read();

            IMonitorifyService service = new MonitorifyService();

            service.ListenerStarted += endpoint => System.Console.WriteLine($"Url listener {endpoint.Name} is Started");
            service.Online += endpoint => System.Console.WriteLine($"Endpoint for {endpoint.Name} is Online");
            service.Offline += endpoint => System.Console.WriteLine($"Endpoint for {endpoint.Name} is Offline!!!");
            service.ErrorOccured += ex => System.Console.WriteLine($"Error occured!!! {ex.Message}");
            service.ListenerEnded += endpoint => System.Console.WriteLine($"Url listener {endpoint.Name} is Ended for Url {endpoint.Url}");

            service.WentOffline += endpoint => System.Console.WriteLine($"Url {endpoint.Name} {endpoint.Url} went offline!");
            service.BackOnline += endpoint => System.Console.WriteLine($"Url {endpoint.Name} {endpoint.Url} is back online, outage time span - {endpoint.LastOutageTimeSpan.Value.ToString(@"d\.hh\:mm\:ss")}");

            service.Start(configuration).Wait();

            while (true) {
                Thread.Sleep(5000);
            }
        }
    }
}
