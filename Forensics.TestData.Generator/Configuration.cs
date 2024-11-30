using CommandLine;

namespace Forensics.TestData.Generator;

[Verb("Registry", HelpText = "Convert a registry Key to a json File")]
public class RegistryOptions
{
    [Option('o', "output", Required = false, HelpText = "The path to write the ")]
    public string? OutputPath { get; set; }

    [Option('r', "registryKey", Required = true, HelpText = "The Root registry key to convert to test data")]
    public string? RegistryKey { get; set; }
}