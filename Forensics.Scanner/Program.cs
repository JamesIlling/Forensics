﻿using Forensics.Data;
using Forensics.Registry.RegistryAbstraction;
using Forensics.Registry.Scanners;
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

            var scanner = provider.GetRequiredService<UsbScanner>();
            var results = scanner.Scan();
            var outputs = provider.GetRequiredService<IEnumerable<IOutput>>();
            foreach (var output in outputs)
            {
                output.Output(results);
            }
        }

        private static void ConfigureServices(ServiceCollection services)
        {

            services.AddSingleton<IRegistryBuilder, RegistryBuilder>();
            services.AddSingleton<IScan<SourcedDictionary<string, string?>>, UsbEnumerationScanner>();
            services.AddSingleton<IScan<SourcedDictionary<string, string?>>, UsbStorageEnumerationScanner>();
            services.AddSingleton<HttpClient>(); services.AddSingleton<IOutput, ConsoleDisplay>();
            services.AddSingleton<IOutput, NetworkSend>(provider =>
                new NetworkSend(provider.GetRequiredService<HttpClient>(), "https://localhost:7082/Scan"));

            services.AddSingleton<UsbScanner>();
        }
    }
}
