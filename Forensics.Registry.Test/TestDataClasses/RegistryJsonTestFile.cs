namespace Forensics.Registry.Test.TestDataClasses
{
    public class RegistryJsonTestFile
    {
        public required JsonRegistryEntry Registry { get; set; }
        public required List<JsonRegistryValue> ExpectedValues { get; set; }
    }
}
