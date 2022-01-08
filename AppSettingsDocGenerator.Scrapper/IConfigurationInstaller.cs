using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppSettingsDocGenerator.Scrapper;

public interface IConfigurationInstaller
{
    IServiceCollection AddConfigurations(IServiceCollection collection, IConfiguration configuration);
}