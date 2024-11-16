using System.Runtime.Versioning;
using FluentAssertions;
using Forensics.Registry.RegistryAbstraction;

namespace Forensics.Registry.Test
{
    public class RegistryBuilderTests
    {
        private const string RootRegistryKey = @"HKLM\SYSTEM\CurrentControlSet\Enum\USB";

        [Theory]
        [SupportedOSPlatform("Windows")]
        [InlineData(RootRegistryKey, "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Enum\\USB")]
        [InlineData("HKCC", "HKEY_CURRENT_CONFIG")]
        [InlineData("HKCR", "HKEY_CLASSES_ROOT")]
        [InlineData("HKCU", "HKEY_CURRENT_USER")]
        [InlineData("HKPD", "HKEY_PERFORMANCE_DATA")]
        [InlineData("HKU", "HKEY_USERS")]
        public void GetRegistry_ReturnsExpectedKey(string keyName, string expected)
        {
            var registry = new RegistryBuilder();
            var key = registry.GetRegistry(keyName);
            key?.Name.Should().Be(expected);
        }


        [Fact]
        [SupportedOSPlatform("Windows")]
        public void GetRegistry_ReturnsNullIfKeyDoesNotExist()
        {
            var registry = new RegistryBuilder();
            var key = registry.GetRegistry(RootRegistryKey.Replace("USB", "USG"));
            key.Should().BeNull();
        }

        [Fact]
        [SupportedOSPlatform("Windows")]
        public void GetRegistry_ReturnsNullIfKeyIsNull()
        {
            var registry = new RegistryBuilder();
            var key = registry.GetRegistry(null);
            key.Should().BeNull();
        }

        [Fact]
        [SupportedOSPlatform("Windows")]
        public void GetRegistry_ReturnsNullIfUnknownHive()
        {
            var registry = new RegistryBuilder();
            var key = registry.GetRegistry(RootRegistryKey.Replace("HKLM", "USG"));
            key.Should().BeNull();
        }
    }
}
