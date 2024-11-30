using System.Runtime.Versioning;

namespace Forensics.Registry.RegistryAbstraction;

public class RegistryBuilder : IRegistryBuilder
{
    private const char Separator = '\\';

    [SupportedOSPlatform("Windows")]
    public IRegistryKey? GetRegistry(string? key)
    {
        if (key == null)
        {
            return null;
        }

        var keyParts = key.Split(Separator).ToList();

        var registryKey = keyParts[0].ToUpper() switch
        {
            "HKLM" => Microsoft.Win32.Registry.LocalMachine,
            "HKCC" => Microsoft.Win32.Registry.CurrentConfig,
            "HKCR" => Microsoft.Win32.Registry.ClassesRoot,
            "HKCU" => Microsoft.Win32.Registry.CurrentUser,
            "HKPD" => Microsoft.Win32.Registry.PerformanceData,
            "HKU" => Microsoft.Win32.Registry.Users,
            _ => null
        };
        keyParts.RemoveAt(0);

        if (registryKey == null)
        {
            return null;
        }

        foreach (var folder in keyParts)
        {
            if (registryKey != null &&
                registryKey.GetSubKeyNames().Contains(folder))
            {
                registryKey = registryKey.OpenSubKey(folder);
            }
            else
            {
                return null;
            }
        }

        return new RegistryKeyKeyWrapper(registryKey);
    }
}