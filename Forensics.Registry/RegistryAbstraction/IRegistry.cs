namespace Forensics.Registry.RegistryAbstraction;

public interface IRegistry
{
    string Name { get; }
    IRegistry? OpenSubKey(string subKeyName);
    string? GetValue(string valueName);
    Guid? GetGuidValue(string valueName);
    string[] GetSubKeyNames();
    IEnumerable<string> GetValueNames();
}