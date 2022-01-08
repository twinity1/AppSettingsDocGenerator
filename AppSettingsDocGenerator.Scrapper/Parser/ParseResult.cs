namespace AppSettingsDocGenerator.Scrapper.Parser;

public class ParseResult
{
    public string Path { get; set; } = default!;

    public string? Description { get; set; }
    
    public string Key { get; set; } = default!;

    public List<ParseResult> Children { get; set; } = new();
    
    public bool IsComplex { get; set; }
    
    public Type ReferenceType { get; set; }
    
    public bool IsArray { get; set; }
}