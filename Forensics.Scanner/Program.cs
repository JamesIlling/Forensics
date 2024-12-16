using Forensics.Data;
using Forensics.Registry.RegistryAbstraction;
using Forensics.Registry.Scanners;
using Forensics.Scanner.Output;
using Forensics.SetupApi;
using Forensics.WindowsManagement;
using Microsoft.Extensions.DependencyInjection;

namespace Forensics.Scanner;

internal static class Program
{
    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<IRegistryBuilder, RegistryBuilder>();
        services.AddSingleton<IScan<SourcedDictionary<string, string?>>, UsbEnumerationScanner>();
        services.AddSingleton<IScan<SourcedDictionary<string, string?>>, UsbStorageEnumerationScanner>();
        services.AddSingleton<IScan<SourcedDictionary<string, string?>>, MountedDevicesScanner>();
        services.AddSingleton<IScan<SourcedDictionary<string, string?>>, SetupApiScanner>();
        services.AddSingleton<IScan<SourcedDictionary<string, string?>>, CurrentlyAttachedScanner>();
        services.AddSingleton<IOutput, ConsoleDisplay>();
        services.AddSingleton<IOutput, FileOutput>();
        services.AddSingleton<UsbScanner>();
    }

    private static void Main()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        var provider = services.BuildServiceProvider();

        var scanner = provider.GetRequiredService<UsbScanner>();
        var results = scanner.Scan();

        var outputs = provider.GetRequiredService<IEnumerable<IOutput>>();
        foreach (var output in outputs)
        {
            output.Output(results);
        }
    }
}