namespace AppSettingsDocGenerator.Scrapper.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class DocsStrategyAttribute : Attribute
{
    public DocsStrategy DocsStrategy { get; }

    public DocsStrategyAttribute(DocsStrategy docsStrategy)
    {
        DocsStrategy = docsStrategy;
    }
}

public enum DocsStrategy
{
    ExcludeAll,
    IncludeAll,
}