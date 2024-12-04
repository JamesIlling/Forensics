using FluentAssertions;
using Forensics.Registry.RegistryAbstraction;
using Forensics.Registry.Scanners;
using Forensics.Registry.Test.TestDataClasses;
using Moq;

namespace Forensics.Registry.Test;

public class UsbEnumerationScannerTests
{
    [Fact]
    public void Name_IsSet()
    {
        var usbscanner = new UsbEnumerationScanner(new Mock<IRegistryBuilder>().Object);
        usbscanner.Name.Should().Be(nameof(UsbEnumerationScanner));
    }

    [Theory]
    [FileData(typeof(RegistryJsonFileReader), "Resources/UsbDeviceRegistry.json")]
    public void Scan_AddsExpectedProperties(IMock<IRegistryBuilder> registry, string key, string? value)
    {
        var usbscanner = new UsbEnumerationScanner(registry.Object);

        var results = usbscanner.Scan();
        results.Count.Should().Be(1);
        var device = results[0];
        device.First().Source?.ToLower().Should().Contain("\\USB\\".ToLower());
        device.Get(key).Should().Be(value);
    }
}