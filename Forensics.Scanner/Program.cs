using Forensics.Registry.RegistryAbstraction;
using Forensics.Registry.Scanners;
using Forensics.Registry.SourcedDictionary;
using Forensics.Scanner.Output;
using Microsoft.Extensions.DependencyInjection;

namespace Forensics.Scanner
{
    internal static class Program
    {
        private static void Main()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var provider = services.BuildServiceProvider();

            var scanner = provider.GetRequiredService<IScan<SourcedDictionary<string, string?>>>();

            var results = new ScanResults { DeviceList = scanner.Scan() };
            var output = provider.GetRequiredService<IOutput>();
            output.Output(results);
        }

        private static void ConfigureServices(ServiceCollection services)
        {

            services.AddSingleton<IRegistryBuilder, RegistryBuilder>();
            services.AddSingleton<IScan<SourcedDictionary<string, string?>>, UsbEnumerationScanner>();
            services.AddSingleton<IOutput, ConsoleDisplay>();
        }
    }
}
