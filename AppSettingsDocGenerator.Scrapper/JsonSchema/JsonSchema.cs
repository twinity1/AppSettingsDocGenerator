namespace AppSettingsDocGenerator.Scrapper.JsonSchema;

public class JsonSchema : JsonSchemaItem
{
    public JsonSchema()
    {
        Type = "object";
    }

    public string Schema { get; } = "https://json-schema.org/draft/2020-12/schema";
}