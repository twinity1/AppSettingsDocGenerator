using System.Text.Json.Serialization;

namespace AppSettingsDocGenerator.Schema;

public class JsonSchema : JsonSchemaItem
{
    public string? Title { get; set; } = default!;

    public JsonSchema()
    {
        Type = "object";
    }

    [JsonPropertyName("$schema")]
    public string Schema { get; } = "http://json-schema.org/draft-07/schema";
}