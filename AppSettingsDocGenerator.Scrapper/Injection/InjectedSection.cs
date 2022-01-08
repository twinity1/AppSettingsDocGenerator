using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace AppSettingsDocGenerator.Scrapper.Injection;

public class InjectedSection : IConfigurationSection
{
    private readonly InjectedSection? _parentSection;

    public InjectedSection(string key, InjectedSection? parentSection)
    {
        _parentSection = parentSection;
        Key = key;
    }

    public IEnumerable<IConfigurationSection> GetChildren()
    {
        return new List<IConfigurationSection>()
        {
            new InjectedSection("children", this)
        };
    }

    public IChangeToken GetReloadToken()
    {
        throw new NotImplementedException();
    }

    public IConfigurationSection GetSection(string key)
    {
        return new InjectedSection(key, this);
    }

    public string this[string key]
    {
        get => "";
        set => throw new NotImplementedException();
    }

    public string Key { get; }
    public string Path => _parentSection?.Key == null ? Key : $"{_parentSection.Key}.{Key}";
    public string Value { get; set; } = "0";
}