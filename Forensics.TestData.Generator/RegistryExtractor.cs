using System.Drawing;
using System.Text.Json;
using Forensics.Registry.RegistryAbstraction;
using Forensics.Registry.Test.TestDataClasses;
using Pastel;

namespace Forensics.TestData.Generator;

internal class RegistryExtractor
{
    private readonly IRegistryBuilder _registryBuilder;

    public RegistryExtractor(IRegistryBuilder registryBuilder)
    {
        _registryBuilder = registryBuilder;
    }

    public void ExtractRegistryKey(string? registryKey, string outputPath)
    {
        if (registryKey == null)
        {
            Console.WriteLine("Unable to extract registry key none was provided".Pastel(Color.DarkRed));
            return;
        }

        var rootKey = _registryBuilder.GetRegistry(registryKey);
        if (rootKey == null)
        {
            Console.WriteLine("Unable to retrieve root key, Please Check it exists".Pastel(Color.DarkRed));
            return;
        }

        if (outputPath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
        {
            Console.WriteLine(
                $"Unable to process file path {outputPath} it contains invalid characters".Pastel(Color.DarkRed));
            return;
        }

        var json = AddKey(rootKey);
        var fileData = new RegistryJsonTestFile
        {
            Registry = json,
            ExpectedValues = []
        };

        using var file = File.OpenWrite(outputPath);
        JsonSerializer.Serialize(file, fileData);
    }

    private static JsonRegistryEntry AddKey(IRegistryKey key)
    {
        Console.WriteLine($"Processing {key.Name}".Pastel(Color.DarkOliveGreen));
        var json = new JsonRegistryEntry
        {
            Name = key.Name,
            Values = [],
            SubKeys = []
        };

        foreach (var value in key.GetValueNames())
        {
            json.Values.Add(new JsonRegistryValue { Name = value, Value = key.GetValue(value) ?? "" });
        }

        foreach (var subKeyName in key.GetSubKeyNames())
        {
            var subKey = key.OpenSubKey(subKeyName);
            if (subKey != null)
            {
                json.SubKeys.Add(AddKey(subKey));
            }
        }

        return json;
    }
}