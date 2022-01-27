using AppSettingsDocGenerator.Scrapper;

namespace WebApplication1;

public class ConfigurationInstaller : IConfigurationInstaller
{
    public IServiceCollection AddConfigurations(IServiceCollection collection, IConfiguration configuration)
    {
        collection.ConfigureWithDocs<Config1>(configuration.GetSection("config1"));
        collection.ConfigureWithDocs<NestedSectionConfig>(configuration.GetSection("config1").GetSection("nested"));
        collection.ConfigureWithDocs<Service>(configuration.GetSection("serviceTypes"));
        collection.ConfigureWithDocs<ArrayConfig>(configuration.GetSection("arrayTest"));
        
        return collection;
    }
}

/// <summary>
/// This config contains arrays
/// </summary>
public class ArrayConfig
{
    /// <summary>
    /// Set of valid urls
    /// </summary>
    public IEnumerable<string> Urls { get; set; }
    
    /// <summary>
    /// Set of services config
    /// </summary>
    public ICollection<Service> Services { get; set; }
    
    /// <summary>
    /// Set of secret services
    /// </summary>
    public Service[] SecretsServices { get; set; }
    
    /// <summary>
    /// Set of secret words
    /// </summary>
    public string[] SecretsWords { get; set; }
}

/// <summary>
/// This section is nested!
/// </summary>
public class NestedSectionConfig
{
    /// <summary>
    /// This is nested property with description
    /// </summary>
    public bool NestedProperty1 { get; set; } = true;
    
    public string NestedProperty2 { get; set; }
    
    public int NestedProperty3 { get; set; }
    
    public float NestedProperty4 { get; set; }
    
    public double NestedProperty5 { get; set; }
    
    public decimal NestedProperty6 { get; set; }
}

public class Service
{
    /// <summary>
    /// Type of the service
    /// </summary>
    public ServiceType Type { get; set; }
    
    /// <summary>
    /// Service name
    /// </summary>
    public string Name { get; set; }
}

public enum ServiceType
{
    One,
    Two,
    Three
}

/// <summary>
/// This is config 1
/// </summary>
public class Config1
{
    /// <summary>
    /// Ahoy!
    /// </summary>
    public string XX { get; set; }
    
    public string YY { get; set; }
    
    public Config2 RandomNestedConfig { get; set; }
}

/// <summary>
/// This is config 2
/// </summary>
public class Config2
{
    /// <summary>
    /// This is random URL
    /// </summary>
    public string Url { get; } = "https://defaulturl.com";
}