using CommandLine;

namespace AppSettingsDocGenerator;

public class ProgramArgs
{
    [Value(0)]
    public string TargetProject { get; set; } = default!;
    
    [Option('o', "output", Required = false, HelpText = "Output path")]
    public string? Output { get; set; }
    
    [Option("title", Required = false, HelpText = "Title")]
    public string? Title { get; set; }

    [Option('t', "output-type", Required = false, HelpText = "MdFlat | JsonSchema")]
    public OutputType OutputType { get; set; } = OutputType.MdFlat;
}

public enum OutputType
{
    MdFlat,
    JsonSchema
}