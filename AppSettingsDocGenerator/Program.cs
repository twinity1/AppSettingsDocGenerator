using AppSettingsDocGenerator;
using AppSettingsDocGenerator.AssemblyHelper;
using AppSettingsDocGenerator.Building;
using AppSettingsDocGenerator.Schema;
using AppSettingsDocGenerator.Templates;
using CommandLine;

var parserResult = Parser.Default.ParseArguments<ProgramArgs>(args);

if (parserResult is NotParsed<ProgramArgs>)
{
    return 1;
}

var programArgs = ((Parsed<ProgramArgs>)parserResult).Value;

using var projectBuilder = new ProjectBuilder();
var dllPath = projectBuilder.Build(programArgs.TargetProject);

var executorAssemblyName = "AppSettingsDocGenerator.Scrapper";

var assemblyLoader = new AssemblyLoader();
var (scrapperAssembly, targetAssembly) = assemblyLoader.Load(dllPath, executorAssemblyName);

var executor = scrapperAssembly.GetType("AppSettingsDocGenerator.Scrapper.Executor");

var instance = Activator.CreateInstance(executor);

try
{
    var jsonSchemaString = executor
        .GetMethod("Execute")
        .Invoke(instance, new object?[] {targetAssembly});

    if (jsonSchemaString is string schemaString)
    {
        var jsonSchema = new JsonSchemaDeserializer().Deserialize(schemaString);

        jsonSchema.Title = programArgs.Title;

        var templateGenerator = new TemplateGeneratorResolver().Create(programArgs.OutputType);

        File.WriteAllText(Path.GetFullPath(programArgs.Output ?? $"docs.{templateGenerator.DefaultFileExtension()}"), templateGenerator.Generate(jsonSchema));
    }
    else
    {
        Console.WriteLine("Schema is not valid");
    }

    return 0;
}
catch(Exception e)
{
    return 1;
}





