using System.Text;
using AppSettingsDocGenerator.Schema;

namespace AppSettingsDocGenerator.Templates.MdFlatGenerator;

public class MdFlatTemplateGenerator : ITemplateGenerator
{
    public string DefaultFileExtension()
    {
        return "MD";
    }

    public string Generate(JsonSchema jsonSchema)
    {
        var result = new StringBuilder();
        
        if (jsonSchema.Title != null)
        {
            result.Append($"{jsonSchema.Title}\n==========\n");
        }
        
        foreach (var (key, item) in jsonSchema.Properties ?? new())
        {
            AddItems(result, item, key);
        }

        return result.ToString();
    }

    private void AddItems(StringBuilder output, JsonSchemaItem jsonSchemaItem, string path, bool showTitle = true)
    {
        if (jsonSchemaItem.Properties != null)
        {
            if (showTitle)
            {
                output.Append($"\n{path}\n--------\n");

                var descriptionOfSection = jsonSchemaItem.Description;

                if (descriptionOfSection != null)
                {
                    output.Append($"{descriptionOfSection}\n");
                }

                output.Append("\n");
            }
            
            foreach (var (key, jsonItem) in jsonSchemaItem.Properties)
            {
                AddItems(output, jsonItem, $"{path}.{key}");

                if (jsonItem.Items != null)
                {
                    AddItems(output, jsonItem.Items, $"{path}.{key}[..]", false);
                }
            }
            return;
        }

        output.Append($"- `{path}` *({jsonSchemaItem.Type})*{(jsonSchemaItem.Enum != null ? (" - " + string.Join(",", jsonSchemaItem.Enum) + " ") : "")}");

        if (jsonSchemaItem.Description != null)
        {
            output.Append($" => {jsonSchemaItem.Description?.ReplaceLineEndings("")}");
        }
        
        output.Append("\n");
    }
}