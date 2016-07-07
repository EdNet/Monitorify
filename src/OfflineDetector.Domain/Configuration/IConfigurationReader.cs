namespace OfflineDetector.Domain.Configuration
{
    public interface IConfigurationReader
    {
        void SetSource(string sourcePath);
        IConfiguration Read();
    }
}
