using System.Text.Json;

namespace AppSettingsDocGenerator.Schema;

public class JsonSchemaDeserializer
{
    public JsonSchema Deserialize(string schema)
    {
        return JsonSerializer.Deserialize<JsonSchema>(schema, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        })!;
    }
}