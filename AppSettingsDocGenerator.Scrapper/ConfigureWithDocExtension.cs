using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppSettingsDocGenerator.Scrapper;

public static class ConfigureWithDocExtension
{
    public static readonly Dictionary<Type, IConfigurationSection> Configurations = new();
    
    public static IServiceCollection ConfigureWithDocs<TOptions>(this IServiceCollection serviceCollection, IConfigurationSection section) where TOptions : class
    {
        serviceCollection.Configure<TOptions>(section);

        Configurations[typeof(TOptions)] = section;
        
        return serviceCollection;
    }
}