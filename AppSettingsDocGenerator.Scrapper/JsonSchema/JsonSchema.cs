using System.Text.Json.Serialization;

namespace AppSettingsDocGenerator.Scrapper.JsonSchema;

public class JsonSchema : JsonSchemaItem
{
    public JsonSchema()
    {
        Type = "object";
    }

    [JsonPropertyName("$schema")]
    public string Schema { get; } = "http://json-schema.org/draft-07/schema";
}