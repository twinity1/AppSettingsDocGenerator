using System.Text.Json;
using System.Text.Json.Serialization;
using AppSettingsDocGenerator.Scrapper.Parser;

namespace AppSettingsDocGenerator.Scrapper.JsonSchema;

public class JsonSchemaGenerator
{
    public string? Title { get; set; }
    
    public Func<string, bool>? IncludeField { get; set; }

    public string Generate(IEnumerable<ParseResult> parseResults)
    {
        var schema = new Scrapper.JsonSchema.JsonSchema
        {
            Properties = new()
        };
        
        foreach (var parseResult in parseResults)
        {
            CreateItem(parseResult.Path, schema, parseResult);
        }

        return JsonSerializer.Serialize(schema, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }

    private void CreateItem(string path, JsonSchemaItem parentItem, ParseResult parseResult)
    {
        if (IncludeField?.Invoke(path) == false)
        {
            return;
        }
        
        var keys = path.Split(".");
        
        if (parentItem.Properties == null)
        {
            parentItem.Properties = new();
        }

        if (keys.Length == 1)
        {
            var jsonItem = CreateItem(parseResult);

            if (parentItem.Properties.ContainsKey(keys.First()))
            {
                var existingProperties = parentItem.Properties[keys.First()];
                
                parentItem.Properties[keys.First()] = jsonItem;

                foreach (var (existingKey, existingItem) in existingProperties.Properties ?? new())
                {
                    jsonItem.Properties[existingKey] = existingItem;
                }
            }
            else
            {
                parentItem.Properties[keys.First()] = jsonItem;
            }
            
            return;
        }

        var nextKey = keys.First();

        var newParent = new JsonSchemaItem
        {
            Type = "object",
            Properties = new(),
        };
        
        if (parentItem.Properties.ContainsKey(nextKey))
        {
            newParent = parentItem.Properties[nextKey];
        }
        else
        {
            parentItem.Properties[nextKey] = newParent;
        }
        
        CreateItem(string.Join(",", keys.Skip(1).ToArray()), newParent, parseResult);
    }
    
    private JsonSchemaItem CreateItem(ParseResult parseResult, bool checkArray = true, bool setDescriptionNull = false)
    {
        if (parseResult.IsArray && checkArray)
        {
            return new JsonSchemaItem
            {
                Type = "array",
                Description = parseResult.Description,
                Items = CreateItem(parseResult, false, true)
            };
        }
        
        return new()
        {
            Description = setDescriptionNull ? null : parseResult.Description,
            Type = ResolveType(parseResult),
            Enum = (parseResult.ReferenceType?.IsEnum ?? false) switch {
                true => Enum.GetNames(parseResult.ReferenceType),
                false => null
            },
            Properties = parseResult.IsComplex switch
            {
                true => parseResult.Children.ToDictionary(x => x.Key, result => CreateItem(result)),
                false => null
            }
        };
    }

    private string ResolveType(ParseResult parseResult)
    {
        if (parseResult.IsComplex)
        {
            return "object";
        }

        return GetType(parseResult.ReferenceType);
    }

    private string GetType(Type type)
    {
        if (
            type == typeof(int) ||
            type == typeof(uint) ||
            type == typeof(long) ||
            type == typeof(ulong) ||
            type == typeof(short) ||
            type == typeof(ushort) ||
            type == typeof(byte) ||
            type == typeof(sbyte) ||
            type == typeof(byte)
            )
        {
            return "integer";
        }

        if (type == typeof(float) || type == typeof(decimal) || type == typeof(double))
        {
            return "number";
        }
        
        if (type == typeof(bool))
        {
            return "boolean";
        }

        return "string";
    }
}