using AppSettingsDocGenerator.Templates.JsonSchemaGenerator;
using AppSettingsDocGenerator.Templates.MdFlatGenerator;

namespace AppSettingsDocGenerator.Templates;

public class TemplateGeneratorResolver
{
    public ITemplateGenerator Create(OutputType outputType)
    {
        return outputType switch
        {
            OutputType.MdFlat => new MdFlatTemplateGenerator(),
            OutputType.JsonSchema => new JsonSchemaTemplateGenerator(),
            _ => throw new ArgumentOutOfRangeException(nameof(outputType), outputType, "Invalid output type")
        };
    }
}