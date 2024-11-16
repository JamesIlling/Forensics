using System.Runtime.Versioning;
using Microsoft.Win32;

namespace Forensics.Registry.RegistryAbstraction;

[SupportedOSPlatform("windows")]
public class RegistryKeyWrapper : IRegistry
{
    private readonly RegistryKey? _key;

    public RegistryKeyWrapper(RegistryKey? key)
    {
        _key = key;
    }

    public string Name => _key?.Name ?? "";

    public IRegistry? OpenSubKey(string subKeyName)
    {
        if (_key != null && _key.GetSubKeyNames().Contains(subKeyName))
        {
            return new RegistryKeyWrapper(_key.OpenSubKey(subKeyName));
        }

        return null;
    }

    public Guid? GetGuidValue(string valueName)
    {
        if (!GetValueNames().Contains(valueName))
        {
            return null;
        }

        var kind = _key?.GetValueKind(valueName);
        if (kind != RegistryValueKind.String)
        {
            return null;
        }

        var text = _key!.GetValue(valueName)?.ToString();
        if (text == null)
        {
            return null;
        }

        return new Guid(text);
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