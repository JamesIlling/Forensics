using Forensics.Data;
using Forensics.Registry.RegistryAbstraction;

namespace Forensics.Registry.Scanners
{
    public class MountedDevicesScanner : IScan<SourcedDictionary<string, string?>>
    {
        private const string RootRegistryKey = @"HKLM\SYSTEM\MountedDevices";
        private readonly IRegistryBuilder _registryBuilder;

        public MountedDevicesScanner(IRegistryBuilder registryBuilder)
        {
            _registryBuilder = registryBuilder;

        }

        public string Name => nameof(MountedDevicesScanner);

        public List<SourcedDictionary<string, string?>> Scan()
        {
            var located = new List<SourcedDictionary<string, string?>>();
            var rootKey = _registryBuilder.GetRegistry(RootRegistryKey);


            foreach (var valueName in rootKey?.GetValueNames() ?? [])
            {
                var entries = new SourcedDictionary<string, string?>();
                if (rootKey != null)
                {
                    var value = rootKey.GetValue(valueName);

                    entries.Add(rootKey.Name, "Key", valueName);
                    entries.Add(rootKey.Name, "Data", value);
                }

                located.Add(entries);
            }

            return located;
        }
    }


}
