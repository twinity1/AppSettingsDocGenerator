using System.Reflection;

namespace AppSettingsDocGenerator.Scrapper.Documentation;

public interface IDocumentationProvider
{
    public string? GetDocumentation(PropertyInfo propertyInfo);
    
    public string? GetDocumentation(Type classType);
}