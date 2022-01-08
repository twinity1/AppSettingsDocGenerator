using AppSettingsDocGenerator.Schema;

namespace AppSettingsDocGenerator.Templates;

public interface ITemplateGenerator
{
    public string Generate(JsonSchema jsonSchema);

    public string DefaultFileExtension();
}