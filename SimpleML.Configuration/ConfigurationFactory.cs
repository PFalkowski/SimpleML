using Microsoft.Extensions.Configuration;

namespace SimpleML.Configuration
{
    public class ConfigurationFactory
    {
        public IConfiguration GetConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.development.json", optional: true, reloadOnChange: true)
                .Build();

            return configuration;
        }
    }
}
