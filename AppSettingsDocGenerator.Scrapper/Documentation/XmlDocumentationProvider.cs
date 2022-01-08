using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace AppSettingsDocGenerator.Scrapper.Documentation;

public class XmlDocumentationProvider : IDocumentationProvider
{
    private readonly Dictionary<string, string> _xmlDocuments = new();
    private readonly HashSet<Assembly> _loadedAssemblies = new();

    private void LoadXmlDocumentation(string xmlDocumentation)
    {
        using XmlReader xmlReader = XmlReader.Create(new StringReader(xmlDocumentation));

        string? lastElementName = null;
        
        while (xmlReader.Read())
        {
            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "member")
            {
                lastElementName = xmlReader.GetAttribute("name")!;
                _xmlDocuments[lastElementName] = "";
            }

            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "summary")
            {
                _xmlDocuments[lastElementName!] = xmlReader.ReadElementString().Trim();
            }
        }
    }
    
    private static string XmlDocumentationKeyHelper(
        string typeFullNameString,
        string? memberNameString)
    {
        string key = Regex.Replace(
            typeFullNameString, @"\[.*\]",
            string.Empty).Replace('+', '.');
        if (memberNameString != null)
        {
            key += "." + memberNameString;
        }
        return key;
    }
    
    private string GetDirectoryPath(Assembly assembly)
    {
        var codeBase = assembly.CodeBase!;
        var uri = new UriBuilder(codeBase);
        var path = Uri.UnescapeDataString(uri.Path);
        return Path.GetDirectoryName(path)!;
    }
    
    private void LoadXmlDocumentation(Assembly assembly)
    {
        if (_loadedAssemblies.Contains(assembly)) {
            return; // Already loaded
        }

        var directoryPath = GetDirectoryPath(assembly);
        var xmlFilePath = Path.Combine(directoryPath, assembly.GetName().Name + ".xml");
        
        if (File.Exists(xmlFilePath)) {
            LoadXmlDocumentation(File.ReadAllText(xmlFilePath));
        }
        
        _loadedAssemblies.Add(assembly);
    }
    
    public string? GetDocumentation(PropertyInfo propertyInfo)
    {
        LoadXmlDocumentation(propertyInfo.PropertyType.Assembly);
        LoadXmlDocumentation(propertyInfo.ReflectedType.Assembly);
        
        var key = "P:" + XmlDocumentationKeyHelper(propertyInfo.DeclaringType.FullName, propertyInfo.Name);
        _xmlDocuments.TryGetValue(key, out string documentation);
        return documentation;
    }

    public string? GetDocumentation(Type classType)
    {
        LoadXmlDocumentation(classType.Assembly);
        
        var key = "T:" + XmlDocumentationKeyHelper(classType.FullName, null);
        _xmlDocuments.TryGetValue(key, out string documentation);
        return documentation;
    }
}