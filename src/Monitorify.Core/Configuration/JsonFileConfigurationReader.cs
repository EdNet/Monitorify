using System;
using System.IO;
using Newtonsoft.Json;

namespace Monitorify.Core.Configuration
{
    public class JsonFileConfigurationReader : IConfigurationReader
    {
        private string _sourcePath;

        public void SetSource(string sourcePath)
        {
            _sourcePath = sourcePath;
        }

        public IConfiguration Read()
        {
            if (string.IsNullOrEmpty(_sourcePath))
            {
                throw new ArgumentException("Source path cannot be empty", "sourcePath");
            }

            using (StreamReader file = File.OpenText(_sourcePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                IConfiguration configuration = (JsonConfiguration)serializer.Deserialize(file, typeof(JsonConfiguration));

                return configuration;
            }
        }
    }
}