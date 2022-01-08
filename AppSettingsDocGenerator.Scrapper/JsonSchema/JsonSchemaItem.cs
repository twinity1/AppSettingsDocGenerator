namespace AppSettingsDocGenerator.Scrapper.JsonSchema;

public class JsonSchemaItem
{
    public string Type { get; set; } = default!;

    public string? Description { get; set; }
    
    public ICollection<string>? Enum { get; set; }

    public Dictionary<string, JsonSchemaItem>? Properties { get; set; }
    
    public JsonSchemaItem? Items { get; set; }
}