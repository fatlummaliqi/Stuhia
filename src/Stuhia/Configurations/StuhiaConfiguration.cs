using System.Reflection;

namespace Stuhia.Configurations;

public record StuhiaConfiguration
{
    internal List<Assembly> AssembliesToScan;

    public bool SilentFailures { get; set; } = true;
    public bool EnableLogging { get; set; } = true;

    public void RegisterHandlersFromAssembly(Assembly assembly)
    {
        AssembliesToScan.Add(assembly);
    }

    public void RegisterHandlersFromAssemblies(params Assembly[] assemblies)
    {
        AssembliesToScan.AddRange(assemblies);
    }
}
