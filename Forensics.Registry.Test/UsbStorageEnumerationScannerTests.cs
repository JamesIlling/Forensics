using FluentAssertions;
using Forensics.Registry.RegistryAbstraction;
using Forensics.Registry.Scanners;
using Forensics.Registry.Test.TestDataClasses;
using Moq;

namespace Forensics.Registry.Test
{
    public class UsbStorageEnumerationScannerTests
    {
        [Theory]
        [FileData(typeof(RegistryJsonFileReader), "Resources/UsbStorRegistry(CDRom).json")]
        [FileData(typeof(RegistryJsonFileReader), "Resources/UsbStorRegistry(USB).json")]
        public void Scan_AddsExpectedProperties(IMock<IRegistryBuilder> registry, string key, string? value)
        {
            var usbscanner = new UsbStorageEnumerationScanner(registry.Object);

            var results = usbscanner.Scan();
            results.Count.Should().Be(1);
            var device = results[0];
            device.First().Source.ToLower().Should().Contain("\\USBStor\\".ToLower());

            var propertyKey = device.FirstOrDefault(x => x.Key == key);
            propertyKey.Should().NotBeNull();
            propertyKey.Value.Should().Be(value);
        }
    }
}




