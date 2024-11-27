namespace Forensics.Registry.Test.TestDataClasses
{
    public class RegistryJsonTestFile
    {
        public JsonRegistryEntry Registry { get; set; }
        public List<JsonRegistryValue> ExpectedValues { get; set; }
    }
}
