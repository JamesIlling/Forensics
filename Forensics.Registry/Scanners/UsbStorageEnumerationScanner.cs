using System.Runtime.Versioning;
using Forensics.Data;
using Forensics.Registry.RegistryAbstraction;

namespace Forensics.Registry.Scanners
{
    public class UsbStorageEnumerationScanner : IScan<SourcedDictionary<string, string?>>
    {
        private const string KeyName = @"HKLM\SYSTEM\CurrentControlSet\Enum\USBSTOR";
        private const string DeviceParameters = "Device Parameters";
        private const string PartitionManagerSubKeyName = "Partmgr";
        private const string ServiceValueName = "Service";
        private const string DiskServiceName = "disk";
        private const string DiskIdValueName = "DiskId";
        private const string DiskIdProperty = "DiskId";

        private readonly IRegistryBuilder _registryBuilder;

        public UsbStorageEnumerationScanner(IRegistryBuilder registryBuilder)
        {
            _registryBuilder = registryBuilder;
        }


        public string Name => "UsbStorageEnumerationScanner";

        [SupportedOSPlatform("windows")]
        public List<SourcedDictionary<string, string?>> Scan()
        {
            var entries = new List<SourcedDictionary<string, string?>>();
            var usbStore = _registryBuilder.GetRegistry(KeyName);
            var deviceTypesKeyNames = usbStore?.GetSubKeyNames() ?? [];
            foreach (var deviceType in deviceTypesKeyNames)
            {
                var deviceTypeKey = usbStore?.OpenSubKey(deviceType)!;

                foreach (var deviceInstance in deviceTypeKey.GetSubKeyNames())
                {
                    var deviceInstanceKey = deviceTypeKey.OpenSubKey(deviceInstance)!;

                    var entry = new SourcedDictionary<string, string?>
                    {
{ deviceTypeKey.Name, "DeviceId", deviceType },
                        { deviceInstanceKey.Name, "DeviceInstanceId", deviceInstance }
                    };


                    foreach (var valueName in deviceInstanceKey.GetValueNames())
                    {
                        entry.Add(deviceInstanceKey.Name, valueName, deviceInstanceKey.GetValue(valueName)!);
                    }

                    var service = deviceInstanceKey.GetValue(ServiceValueName);
                    if (service == DiskServiceName)
                    {
                        GetDiskId(deviceInstanceKey, entry);
                    }

                    entries.Add(entry);
                }
            }
            return entries;
        }

        private static void GetDiskId(IRegistryKey deviceKey, SourcedDictionary<string, string?> entry)
        {
            var deviceProperties = deviceKey.OpenSubKey(DeviceParameters);
            var partManager = deviceProperties?.OpenSubKey(PartitionManagerSubKeyName)!;
            var diskId = partManager.GetValue(DiskIdValueName);
            entry.Add(partManager.Name, DiskIdProperty, diskId);
        }
    }
}
