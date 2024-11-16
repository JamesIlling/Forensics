using Forensics.Registry.RegistryAbstraction;
using Forensics.Registry.SourcedDictionary;

namespace Forensics.Registry.Scanners;

public class UsbEnumerationScanner : IScan<SourcedDictionary<string, string?>>
{
    private const string RootRegistryKey = @"HKLM\SYSTEM\CurrentControlSet\Enum\USB";
    private readonly IRegistryBuilder _registryBuilder;

    public UsbEnumerationScanner(IRegistryBuilder registryBuilder)
    {
        _registryBuilder = registryBuilder;

    }

    public const string ClassGuid = "ClassGUID";
    public const string HardwareId = "HardwareID";
    public const string VendorId = "VendorId";
    public const string ProductId = "ProductId";
    public const string Revision = "Revision";
    public const string InterfaceId = "InterfaceId";
    public const string DeviceInstance = "DeviceInstanceId";
    public const string Service = "Service";
    public const string FriendlyName = "FriendlyName";
    public const string ContainerId = "ContainerID";
    private const string UshortHex = "X04";
    private const string ByteHex = "X02";


    public List<SourcedDictionary<string, string?>> Scan()
    {
        var located = new List<SourcedDictionary<string, string?>>();
        var rootKey = _registryBuilder.GetRegistry(RootRegistryKey);
        foreach (var subKeyName in rootKey?.GetSubKeyNames() ?? [])
        {
            //This is the top level key (DeviceClass)
            //e.g. HKLM\SYSTEM\CurrentControlSet\Enum\USB\ROOT_HUB30\
            var enumeratedKey = rootKey?.OpenSubKey(subKeyName);
            if (enumeratedKey != null)
            {
                foreach (var enumeratedSubKeyName in enumeratedKey.GetSubKeyNames())
                {
                    //This is the key we will want to read the values from (DeviceInstance)
                    //e.g. HKLM\SYSTEM\CurrentControlSet\Enum\USB\ROOT_HUB30\4&92b3c53&0&0
                    var enumeratedSubKey = enumeratedKey.OpenSubKey(enumeratedSubKeyName);
                    var device = new SourcedDictionary<string, string?>();
                    if (enumeratedSubKey != null)
                    {
                        device.Add(enumeratedSubKey.Name, ClassGuid,
                            enumeratedSubKey.GetGuidValue(ClassGuid).ToString());
                        device.Add(enumeratedSubKey.Name, HardwareId, enumeratedSubKey.GetValue(HardwareId));
                        device.Add(enumeratedSubKey.Name, VendorId,
                            enumeratedSubKey.GetValue(HardwareId).ParseVendorId()?.ToString(UshortHex));
                        device.Add(enumeratedSubKey.Name, ProductId,
                            enumeratedSubKey.GetValue(HardwareId).ParseProductId()?.ToString(UshortHex));
                        device.Add(enumeratedSubKey.Name, Revision,
                            enumeratedSubKey.GetValue(HardwareId).ParseRevision()?.ToString(UshortHex));
                        device.Add(enumeratedSubKey.Name, InterfaceId,
                            enumeratedSubKey.GetValue(HardwareId).ParseInterface()?.ToString(ByteHex));
                        device.Add(enumeratedSubKey.Name, DeviceInstance, enumeratedSubKeyName);
                        device.Add(enumeratedSubKey.Name, Service, enumeratedSubKey.GetValue(Service));
                        device.Add(enumeratedSubKey.Name, FriendlyName, enumeratedSubKey.GetValue(FriendlyName));
                        device.Add(enumeratedSubKey.Name, ContainerId, enumeratedSubKey.GetValue(ContainerId));
                        located.Add(device);
                    }
                }
            }
        }

        return located;
    }
}