using CommandLine;
using Forensics.Registry.RegistryAbstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Forensics.TestData.Generator;

internal static class Program
{
    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<IRegistryBuilder, RegistryBuilder>();
        services.AddSingleton<RegistryExtractor>();
    }

    private static int ExtractRegistryKey(RegistryOptions opts, ServiceProvider provider)
    {
        var extractor = provider.GetRequiredService<RegistryExtractor>();
        extractor.ExtractRegistryKey(opts.RegistryKey, opts.OutputPath ?? "TestData.json");
        return 0;
    }

    private static void Main(string[] args)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        var provider = services.BuildServiceProvider();

        Parser.Default.ParseArguments<RegistryOptions>(args)
            .MapResult(
                opts => ExtractRegistryKey(opts, provider),
                errs => 1);
    }
}