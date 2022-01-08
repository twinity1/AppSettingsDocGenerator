using System.Reflection;
using AppSettingsDocGenerator.Scrapper.Documentation;
using AppSettingsDocGenerator.Scrapper.Injection;
using AppSettingsDocGenerator.Scrapper.JsonSchema;
using AppSettingsDocGenerator.Scrapper.Parser;
using Microsoft.Extensions.DependencyInjection;

namespace AppSettingsDocGenerator.Scrapper;

public class Executor
{
    public string Execute(Assembly targetAssembly)
    {
        Type? type = null;

        foreach (var assemblyType in targetAssembly.GetTypes())
        {
            if (assemblyType.IsAssignableTo(typeof(IConfigurationInstaller)))
            {
                type = assemblyType;
                break;
            }
        }

        if (type == null)
        {
            throw new ConfigurationInstallerNotFoundException($"Interface '{typeof(IConfigurationInstaller).FullName}' was not found. Create class of your choice with the implemented interface.");
        }

        var configurationInjection = new ConfigurationInjection();
        IConfigurationInstaller configurationInstaller;

        if (type.GetConstructors().First().GetParameters().Length == 1)
        {
            configurationInstaller = Activator.CreateInstance(type, new object?[] {configurationInjection}) as IConfigurationInstaller;
        }
        else
        {
            configurationInstaller = Activator.CreateInstance(type) as IConfigurationInstaller;   
        }
        
        var serviceCollection = new ServiceCollection();
        
        configurationInstaller.AddConfigurations(serviceCollection, configurationInjection);

        var configurations = ConfigureWithDocExtension.Configurations;

        var xmlDocumentationProvider = new XmlDocumentationProvider();

        var parser = new ConfigurationParser(xmlDocumentationProvider, configurations);

        var items = parser.Parse();

        var jsonSchemaGenerator = new JsonSchemaGenerator();
        
        return jsonSchemaGenerator.Generate(items);
    }
}