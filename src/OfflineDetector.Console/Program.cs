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

            System.Console.ReadLine();
        }
    }
}
