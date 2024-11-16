using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        private static Mock<IRegistry> SetUpRootKey()
        {
            var mock = new Mock<IRegistry>();
            mock.Setup(x => x.Name).Returns("HKLM\\SYSTEM\\CurrentControlSet\\Enum\\USB");
            mock.Setup(x => x.GetSubKeyNames()).Returns(["ROOT_HUB30"]);
            mock.Setup(x => x.OpenSubKey(It.IsAny<string>())).Returns(SetUpDeviceKey().Object);
            return mock;
        }

        private static Mock<IRegistry> SetUpDeviceKey()
        {
            var mock = new Mock<IRegistry>();
            mock.Setup(x => x.Name).Returns("HKLM\\SYSTEM\\CurrentControlSet\\Enum\\USB\\ROOT_HUB30");
            mock.Setup(x => x.GetSubKeyNames()).Returns(["4&92b3c53&0&0"]);
            mock.Setup(x => x.OpenSubKey(It.IsAny<string>())).Returns(SetUpDeviceInstanceKey().Object);
            return mock;
        }

        private static Mock<IRegistry> SetUpDeviceInstanceKey()
        {
            var mock = new Mock<IRegistry>();
            mock.Setup(x => x.Name).Returns("HKLM\\SYSTEM\\CurrentControlSet\\Enum\\USB\\ROOT_HUB30\\4&92b3c53&0&0");

            mock.Setup(x => x.GetGuidValue(UsbEnumerationScanner.ClassGuid))
                .Returns(new Guid("{36fc9e60-c465-11cf-8056-444553540000}"));
            mock.Setup(x => x.GetValue(UsbEnumerationScanner.HardwareId)).Returns(
                "USB\\ROOT_HUB30&VID8086&PIDA2AF&REV0000, USB\\ROOT_HUB30&VID8086&PIDA2AF, USB\\ROOT_HUB30");
            mock.Setup(x => x.GetValue(UsbEnumerationScanner.DeviceInstance))
                .Returns("HKLM\\SYSTEM\\CurrentControlSet\\Enum\\USB\\ROOT_HUB30\\4&92b3c53&0&0");
            mock.Setup(x => x.GetValue(UsbEnumerationScanner.Service)).Returns("USBHUB3");
            mock.Setup(x => x.GetValue(UsbEnumerationScanner.FriendlyName)).Returns("Test");
            mock.Setup(x => x.GetValue(UsbEnumerationScanner.ContainerId)).Returns("{00000000-0000-0000-ffff-ffffffffffff}");
            return mock;
        }


        [Theory]
        [InlineData(UsbEnumerationScanner.HardwareId,
            "USB\\ROOT_HUB30&VID8086&PIDA2AF&REV0000, USB\\ROOT_HUB30&VID8086&PIDA2AF, USB\\ROOT_HUB30")]
        [InlineData(UsbEnumerationScanner.VendorId, "8086")]
        [InlineData(UsbEnumerationScanner.ProductId, "A2AF")]
        [InlineData(UsbEnumerationScanner.Revision, "0000")]
        [InlineData(UsbEnumerationScanner.InterfaceId, null)]
        [InlineData(UsbEnumerationScanner.DeviceInstance, "4&92b3c53&0&0")]
        [InlineData(UsbEnumerationScanner.Service, "USBHUB3")]
        [InlineData(UsbEnumerationScanner.FriendlyName, "Test")]
        [InlineData(UsbEnumerationScanner.ContainerId, "{00000000-0000-0000-ffff-ffffffffffff}")]

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



