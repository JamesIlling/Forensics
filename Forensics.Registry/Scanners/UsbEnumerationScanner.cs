using Forensics.Data;
using Forensics.Registry.RegistryAbstraction;

namespace Forensics.Registry.Scanners;

public class UsbEnumerationScanner : IScan<SourcedDictionary<string, string?>>
{
    private const string DeviceTypeId = "DeviceTypeId";
    private const string DeviceInstanceId = "DeviceInstanceId";
    private readonly IRegistryBuilder _registryBuilder;

    public UsbEnumerationScanner(IRegistryBuilder registryBuilder)
    {
        _registryBuilder = registryBuilder;
    }

    public string Name => "UsbEnumerationScanner";

    public List<SourcedDictionary<string, string?>> Scan()
    {
        var located = new List<SourcedDictionary<string, string?>>();
        foreach (var rootKey in GetKeys())
        {
            foreach (var deviceTypeName in rootKey.GetSubKeyNames())
            {
                //This is the top level key (DeviceClass)
                //e.g. HKLM\SYSTEM\CurrentControlSet\Enum\USB\ROOT_HUB30\
                var deviceTypeKey = rootKey?.OpenSubKey(deviceTypeName);
                foreach (var deviceInstanceName in deviceTypeKey?.GetSubKeyNames() ?? [])
                {
                    //This is the key we will want to read the values from (DeviceInstance)
                    //e.g. HKLM\SYSTEM\CurrentControlSet\Enum\USB\ROOT_HUB30\4&92b3c53&0&0
                    var deviceInstanceKey = deviceTypeKey.OpenSubKey(deviceInstanceName);
                    var device = new SourcedDictionary<string, string?>();
                    if (deviceInstanceKey != null)
                    {
                        device.Add(deviceTypeKey.Name, DeviceTypeId, deviceTypeName);
                        device.Add(deviceInstanceKey.Name, DeviceInstanceId, deviceInstanceName);
                        foreach (var valueName in deviceInstanceKey.GetValueNames())
                        {
                            device.Add(deviceInstanceKey.Name, valueName, deviceInstanceKey.GetValue(valueName)!);
                        }

                        located.Add(device);
                    }

                }
            }
        }

        return located;
    }


    private IRegistryKey[] GetKeys()
    {
        var keys = new List<IRegistryKey>();
        IRegistryKey? key;
        var id = 001;
        do
        {
            var keyName = $@"HKLM\SYSTEM\ControlSet{id:000}\Enum\USB";
            key = _registryBuilder.GetRegistry(keyName);
            id += 1;
            if (key != null)
            {
                keys.Add(key);
            }
        }
        while (key != null);

        return keys.ToArray();
    }
}