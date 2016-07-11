namespace OfflineDetector.Core.Configuration
{
    public interface IConfigurationReader
    {
        void SetSource(string sourcePath);
        IConfiguration Read();
    }
}
