using System.Reflection;
using AppSettingsDocGenerator.Scrapper.Attributes;
using AppSettingsDocGenerator.Scrapper.Documentation;
using Microsoft.Extensions.Configuration;

namespace AppSettingsDocGenerator.Scrapper.Parser;

public class ConfigurationParser
{
    public class SectionParseErrorException : Exception
    {
        public SectionParseErrorException(string message): base(message) { }
    }

    private const int MaxDepth = 80;

    private class MaxDepthReachedException : Exception { }
    
    private readonly IDocumentationProvider _documentationProvider;
    private readonly Dictionary<Type, IConfigurationSection> _configurations;

    private int _currentDepth = 0;

    public ConfigurationParser(IDocumentationProvider documentationProvider, Dictionary<Type, IConfigurationSection> configurations)
    {
        _documentationProvider = documentationProvider;
        _configurations = configurations;
    }
    
    public IEnumerable<ParseResult> Parse()
    {
        var result = new List<ParseResult>();

        foreach (var (type, configurationSection) in _configurations)
        {
            try
            {
                result.Add(ParseConfiguration(type, configurationSection));
            }
            catch (Exception e)
            {
                throw new SectionParseErrorException($"Section '{configurationSection.Key}' could not be parsed. {e.Message}");
            }
        }
        
        return result;
    }

    private IEnumerable<PropertyInfo> GetPropertiesForParse(Type type)
    {
        DocsStrategy? strategy = null;

        var docsStrategyAttribute = type.GetCustomAttribute<DocsStrategyAttribute>();

        if (docsStrategyAttribute != null)
        {
            strategy = docsStrategyAttribute.DocsStrategy;
        }

        return strategy switch
        {
            null => type.GetProperties(),
            DocsStrategy.ExcludeAll => type.GetProperties().Where(x => x.GetCustomAttribute<DocsIncludeAttribute>() != null),
            DocsStrategy.IncludeAll => type.GetProperties().Where(x => x.GetCustomAttribute<DocsExcludeAttribute>() == null),
            _ => throw new ArgumentOutOfRangeException(nameof(type), "Strategy not found")
        };
    }

    private ParseResult ParseConfiguration(Type type, IConfigurationSection configurationSection)
    {
        var children = new List<ParseResult>();
        
        foreach (var propertyInfo in GetPropertiesForParse(type))
        {
            try
            {
                var parseResult = ParseProperty(propertyInfo);
                children.Add(parseResult);
            }
            catch (MaxDepthReachedException)
            {
                _currentDepth = 0;
            }
        }

        return new ParseResult
        {
            Children = children,
            Description = _documentationProvider.GetDocumentation(type),
            Key = configurationSection.Key,
            Path = configurationSection.Path,
            IsComplex = true
        };
    }

    private bool IsComplexType(Type type)
    {
        return
            type.IsClass
            && type.IsPrimitive == false
            && type.IsArray == false
            && type != typeof(string);
    }

    private ParseResult ParseProperty(PropertyInfo propertyInfo)
    {
        _currentDepth++;

        if (_currentDepth >= MaxDepth)
        {
            throw new MaxDepthReachedException();
        }
        
        var children = new List<ParseResult>();

        string? description;

        var isComplex = false;
        var isArray = false;
        var referenceType = propertyInfo.PropertyType;
        
        if (IsComplexType(propertyInfo.PropertyType))
        {
            foreach (var childPropertyInfo in propertyInfo.PropertyType.GetProperties())
            {
                var parseResult = ParseProperty(childPropertyInfo);
            
                children.Add(parseResult);
            }

            description = _documentationProvider.GetDocumentation(propertyInfo) ?? _documentationProvider.GetDocumentation(propertyInfo.PropertyType);

            isComplex = true;
        }
        else
        {
            if (propertyInfo.PropertyType.IsGenericType)
            {
                isArray = true;
                referenceType = propertyInfo.PropertyType.GetGenericArguments().First();

                if (IsComplexType(referenceType))
                {
                    isComplex = true;
                    
                    foreach (var childPropertyInfo in referenceType.GetProperties())
                    {
                        var parseResult = ParseProperty(childPropertyInfo);
            
                        children.Add(parseResult);
                    }
                }
            }
            
            description = _documentationProvider.GetDocumentation(propertyInfo);
        }

        _currentDepth--;
        
        return new ParseResult
        {
            Key = propertyInfo.Name,
            Description = description,
            Children = children,
            IsComplex = isComplex,
            ReferenceType = referenceType,
            IsArray = isArray
        };
    }
}