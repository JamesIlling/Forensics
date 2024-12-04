namespace Forensics.Registry.RegistryAbstraction;

public interface IRegistryKey
{
    string Name { get; }
    string[] GetSubKeyNames();
    string? GetValue(string valueName);
    IEnumerable<string> GetValueNames();
    IRegistryKey? OpenSubKey(string subKeyName);
}