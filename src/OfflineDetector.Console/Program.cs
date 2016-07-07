using System.IO;
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
            var endpoints = provider.Read();

            System.Console.ReadLine();
        }
    }
}
