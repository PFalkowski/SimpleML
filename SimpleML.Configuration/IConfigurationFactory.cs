using Microsoft.Extensions.Configuration;

namespace SimpleML.Configuration;

public interface IConfigurationFactory
{
    IConfiguration GetConfiguration();
}