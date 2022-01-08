using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace AppSettingsDocGenerator.Scrapper.Injection;
    
public class ConfigurationInjection : IConfiguration
{
    public IConfigurationSection GetSection(string key)
    {
        return new InjectedSection(key, null);
    }

    public IEnumerable<IConfigurationSection> GetChildren()
    {
        return new List<IConfigurationSection>();
    }

    public IChangeToken GetReloadToken()
    {
        throw new NotImplementedException();
    }

    public string this[string key]
    {
        get => "{}";
        set => throw new Exception();
    }
}