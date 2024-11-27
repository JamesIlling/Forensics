namespace Forensics.Registry.RegistryAbstraction;

public interface IRegistryKey
{
    string Name { get; }
    IRegistryKey? OpenSubKey(string subKeyName);
    string? GetValue(string valueName);
    string[] GetSubKeyNames();
    IEnumerable<string> GetValueNames();
}