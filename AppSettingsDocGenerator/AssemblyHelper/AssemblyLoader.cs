using System.Reflection;

namespace AppSettingsDocGenerator.AssemblyHelper;

public class AssemblyLoader
{
    public record LoadResult(Assembly ScrapperAssembly, Assembly TargetAssembly);
    
    public LoadResult Load(string dllPath, string scrapperAssemblyName)
    {
        var directory = Path.GetDirectoryName(dllPath);
        var dllFiles = Directory.GetFiles(directory, "*.dll");
        var targetAssemblyName = Path.GetFileNameWithoutExtension(dllPath);
        
        Assembly? scrapperAssembly = null;
        Assembly? targetAssembly = null;

        var allLoadedAssemblies = new List<Assembly>();

        foreach (var dllFile in dllFiles)
        {
            var dllFullPath = Path.GetFullPath(dllFile);
    
            var assembly = Assembly.LoadFile(dllFullPath);

            if (assembly.GetName().Name == scrapperAssemblyName)
            {
                scrapperAssembly = assembly;
            }
            
            if (assembly.GetName().Name == targetAssemblyName)
            {
                targetAssembly = assembly;
            }
    
            allLoadedAssemblies.Add(assembly);
        }

        if (scrapperAssembly == null || targetAssembly == null)
        {
            throw new Exception("Assembly not found");
        }

        AppDomain.CurrentDomain.AssemblyResolve += (sender, eventArgs) =>
        {
            // Console.WriteLine(eventArgs.Name);
            var assembly = allLoadedAssemblies.FirstOrDefault(x => x.FullName == eventArgs.Name) ?? AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName == eventArgs.Name);

            return assembly;
        };
        
        return new LoadResult(scrapperAssembly, targetAssembly);
    }
}