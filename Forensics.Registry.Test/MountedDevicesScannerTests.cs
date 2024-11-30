using FluentAssertions;
using Forensics.Registry.RegistryAbstraction;
using Forensics.Registry.Scanners;
using Forensics.Registry.Test.TestDataClasses;
using Moq;

namespace Forensics.Registry.Test;

public class MountedDevicesScannerTests
{
    private const int ExpectedNumberOfResults = 112;

    [Fact]
    public void Name_IsSet()
    {
        var usbscanner = new MountedDevicesScanner(new Mock<IRegistryBuilder>().Object);
        usbscanner.Name.Should().Be(nameof(MountedDevicesScanner));
    }

    [Theory]
    [FileData(typeof(RegistryJsonFileReader), "Resources/MountedDeviceRegistry.json")]
    public void Scan_AddsExpectedProperties(IMock<IRegistryBuilder> registry, string key, string? value)
    {
        var usbscanner = new MountedDevicesScanner(registry.Object);

        var results = usbscanner.Scan();
        results.Count.Should().Be(ExpectedNumberOfResults);
        var device = results[0];
        device.First().Source?.ToLower().Should().Contain("\\MountedDevices".ToLower());
        device.Get(key).Should().Be(value);
    }
}