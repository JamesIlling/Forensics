using System.Runtime.Versioning;
using System.Security;
using Microsoft.Win32;

namespace Forensics.Registry.RegistryAbstraction;

[SupportedOSPlatform("windows")]
public class RegistryKeyKeyWrapper : IRegistryKey
{
    private readonly RegistryKey? _key;

    public RegistryKeyKeyWrapper(RegistryKey? key)
    {
        _key = key;
    }

    public string Name => _key?.Name ?? "";

    public IRegistryKey? OpenSubKey(string subKeyName)
    {
        if (_key != null &&
            _key.GetSubKeyNames().Contains(subKeyName))
        {
            try
            {
                return new RegistryKeyKeyWrapper(_key.OpenSubKey(subKeyName));
            }
            catch (SecurityException)
            {
                return null;
            }
        }

        return null;
    }

    public string? GetValue(string? valueName)
    {
        if (_key?.GetValueNames().Contains(valueName) ?? false)
        {
            var kind = _key.GetValueKind(valueName);
            switch (kind)
            {
                case RegistryValueKind.ExpandString:
                case RegistryValueKind.String:
                case RegistryValueKind.DWord:
                    return _key.GetValue(valueName)?.ToString();

                case RegistryValueKind.MultiString:
                    return string.Join(", ", ((string[])_key.GetValue(valueName)!).Select(x => $"{{{x}}}"));

                case RegistryValueKind.Binary:

                    return Convert.ToHexString(_key.GetValue(valueName) as byte[] ?? []);
            }
        }

        return null;
    }

    [SupportedOSPlatform("windows")]
    public string[] GetSubKeyNames()
    {
        return _key?.GetSubKeyNames() ?? [];
    }

    public IEnumerable<string> GetValueNames()
    {
        return _key?.GetValueNames() ?? [];
    }
}