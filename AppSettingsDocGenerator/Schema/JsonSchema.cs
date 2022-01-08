namespace AppSettingsDocGenerator.Schema;

public class JsonSchema : JsonSchemaItem
{
    public string? Title { get; set; } = default!;

    public JsonSchema()
    {
        Type = "object";
    }

    public string Schema { get; } = "https://json-schema.org/draft/2020-12/schema";
}