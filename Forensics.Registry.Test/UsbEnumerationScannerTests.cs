using FluentAssertions;
using Forensics.Registry.RegistryAbstraction;
using Forensics.Registry.Scanners;
using Moq;

namespace Forensics.Registry.Test
{
    public class UsbEnumerationScannerTests
    {
        private static Mock<IRegistryBuilder> GetBuilder()
        {
            var builder = new Mock<IRegistryBuilder>();
            builder.Setup(x => x.GetRegistry(It.IsAny<string>())).Returns(SetUpRootKey().Object);
            return builder;
        }


        private static Mock<IRegistryKey> SetUpRootKey()
        {
            var mock = new Mock<IRegistryKey>();
            mock.Setup(x => x.Name).Returns("HKLM\\SYSTEM\\CurrentControlSet\\Enum\\USB");
            mock.Setup(x => x.GetSubKeyNames()).Returns(["ROOT_HUB30"]);
            mock.Setup(x => x.OpenSubKey(It.IsAny<string>())).Returns(SetUpDeviceKey().Object);
            return mock;
        }

        private static Mock<IRegistryKey> SetUpDeviceKey()
        {
            var mock = new Mock<IRegistryKey>();
            mock.Setup(x => x.Name).Returns("HKLM\\SYSTEM\\CurrentControlSet\\Enum\\USB\\ROOT_HUB30");
            mock.Setup(x => x.GetSubKeyNames()).Returns(["4&92b3c53&0&0"]);
            mock.Setup(x => x.OpenSubKey(It.IsAny<string>())).Returns(SetUpDeviceInstanceKey().Object);
            return mock;
        }

        private static Mock<IRegistryKey> SetUpDeviceInstanceKey()
        {
            var mock = new Mock<IRegistryKey>();
            mock.Setup(x => x.Name).Returns("HKLM\\SYSTEM\\CurrentControlSet\\Enum\\USB\\ROOT_HUB30\\4&92b3c53&0&0");
            mock.Setup(x => x.GetValueNames()).Returns(["HardwareId"]);
            mock.Setup(x => x.GetValue("HardwareId")).Returns(
                "USB\\ROOT_HUB30&VID8086&PIDA2AF&REV0000, USB\\ROOT_HUB30&VID8086&PIDA2AF, USB\\ROOT_HUB30");
            return mock;
        }


        [Theory]
        [InlineData("HardwareId",
            "USB\\ROOT_HUB30&VID8086&PIDA2AF&REV0000, USB\\ROOT_HUB30&VID8086&PIDA2AF, USB\\ROOT_HUB30")]

        [InlineData("DeviceTypeId",
            "ROOT_HUB30")]

        [InlineData("DeviceInstanceId",
            "4&92b3c53&0&0\"")]

        public void Scan_AddsExpectedProperties(string key, string? value)
        {
            var usbscanner = new UsbEnumerationScanner(GetBuilder().Object);

            var results = usbscanner.Scan();
            results.Count.Should().Be(1);
            var device = results[0];
            device.FirstOrDefault(x => x.Key == key)?.Value.Should().Be(value);
        }
    }
}



