using System.Text.Json;
using System.Text.Json.Serialization;

namespace AppSettingsDocGenerator.Templates.JsonSchemaGenerator;

public class JsonSchemaTemplateGenerator : ITemplateGenerator
{
    public string Generate(Schema.JsonSchema jsonSchema)
    {
        return JsonSerializer.Serialize(jsonSchema, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }

    public string DefaultFileExtension()
    {
        return "json";
    }
}