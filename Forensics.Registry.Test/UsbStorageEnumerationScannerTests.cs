using System.Runtime.Versioning;
using FluentAssertions;
using Forensics.Registry.RegistryAbstraction;
using Forensics.Registry.Scanners;
using Forensics.Registry.Test.TestDataClasses;
using Moq;

namespace Forensics.Registry.Test
{
    [SupportedOSPlatform("Windows")]
    public class UsbStorageEnumerationScannerTests
    {
        [Fact]
        public void Name_IsSet()
        {
            var usbscanner = new UsbStorageEnumerationScanner(new Mock<IRegistryBuilder>().Object);
            usbscanner.Name.Should().Be(nameof(UsbStorageEnumerationScanner));
        }

        [Theory]
        [FileData(typeof(RegistryJsonFileReader), "Resources/UsbStorRegistry(CDRom).json")]
        [FileData(typeof(RegistryJsonFileReader), "Resources/UsbStorRegistry(USB).json")]
        public void Scan_AddsExpectedProperties(IMock<IRegistryBuilder> registry, string key, string? value)
        {
            var usbScanner = new UsbStorageEnumerationScanner(registry.Object);

            var results = usbScanner.Scan();
            results.Count.Should().Be(1);
            var device = results[0];
            device.First().Source!.ToLower().Should().Contain("\\USBStor\\".ToLower());

            var propertyKey = device.Get(key);
            propertyKey.Should().NotBeNull();
            propertyKey!.Should().Be(value);
        }
    }
}




