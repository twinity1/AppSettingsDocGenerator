using System.Diagnostics;

namespace AppSettingsDocGenerator.Building;

public class ProjectBuilder : IDisposable
{
    private string? _outPath;
    
    /// <summary>
    /// Builds target project if it's csproj
    /// </summary>
    /// <param name="projectPath"></param>
    /// <returns>Target DLL absolute path</returns>
    public string Build(string projectPath)
    {
        if (projectPath.EndsWith(".csproj"))
        {
            var csprojAbsolutePath = Path.GetFullPath(projectPath);
            var outPath = Path.Combine(Path.GetDirectoryName(csprojAbsolutePath), "_docBuildTemp");
            _outPath = outPath;

            var process = Process.Start(new ProcessStartInfo("dotnet", $"build {csprojAbsolutePath} -o {outPath}"));

            process.WaitForExit();

            return Path.Combine(outPath, Path.GetFileNameWithoutExtension(projectPath) + ".dll");
        }

        return Path.GetFullPath(projectPath);
    }

    public void Dispose()
    {
        if (_outPath != null && Directory.Exists(_outPath))
        {
            Directory.Delete(_outPath, true);
        }
    }
}