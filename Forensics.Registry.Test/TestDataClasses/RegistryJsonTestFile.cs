namespace Forensics.Registry.Test.TestDataClasses;

public class RegistryJsonTestFile
{
    public required List<JsonRegistryValue> ExpectedValues { get; set; }
    public required JsonRegistryEntry Registry { get; set; }
}