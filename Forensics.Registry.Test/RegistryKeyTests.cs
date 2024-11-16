using System.Runtime.Versioning;
using FluentAssertions;
using Forensics.Registry.RegistryAbstraction;

namespace Forensics.Registry.Test
{
    [SupportedOSPlatform("Windows")]
    public class RegistryKeyTests
    {
        private const string VersionKey = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion";
        private const string CurrentVersion = @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion";


        [Fact]
        [SupportedOSPlatform("Windows")]
        public void OpenSubKey_OpensKey()
        {
            var registryBuilder = new RegistryBuilder();

            var wrapper = registryBuilder.GetRegistry(VersionKey);
            var entry = wrapper?.OpenSubKey("Uninstall");
            entry.Should().NotBeNull();
        }


        [Fact]
        [SupportedOSPlatform("Windows")]
        public void OpenSubKey_ShouldBeNull_IfKeyDoesNotExist()
        {
            var registryBuilder = new RegistryBuilder();

            var wrapper = registryBuilder.GetRegistry(VersionKey);
            var entry = wrapper?.OpenSubKey("NOPE");
            entry.Should().BeNull();
        }



        [Fact]
        [SupportedOSPlatform("Windows")]
        public void GetSubKeyNames()
        {
            var registryBuilder = new RegistryBuilder();

            var wrapper = registryBuilder.GetRegistry(VersionKey);
            var entry = wrapper?.GetSubKeyNames();

            entry.Should().Contain("Uninstall");
        }

        [Fact]
        [SupportedOSPlatform("Windows")]
        public void GetValueNames_ReturnNamesOfValuesForTheKey()
        {
            var registryBuilder = new RegistryBuilder();

            var wrapper = registryBuilder.GetRegistry(VersionKey);
            var entry = wrapper?.GetValueNames();

            entry.Should().Contain("ProgramFilesPath");
        }


        [Fact]
        [SupportedOSPlatform("Windows")]
        public void GetValue_String()
        {
            var registryBuilder = new RegistryBuilder();

            var wrapper = registryBuilder.GetRegistry(CurrentVersion);
            var entry = wrapper?.GetValue("SystemRoot");

            entry.Should().Contain(@"C:\WINDOWS");
        }

        [Fact]
        [SupportedOSPlatform("Windows")]
        public void GetValue_UnknownValue()
        {
            var registryBuilder = new RegistryBuilder();

            var wrapper = registryBuilder.GetRegistry(CurrentVersion);
            var entry = wrapper?.GetValue("DoesNotExist");

            entry.Should().BeNull();
        }

        [Fact]
        [SupportedOSPlatform("Windows")]
        public void GetGuidValue_UnknownValue()
        {
            var registryBuilder = new RegistryBuilder();

            var wrapper = registryBuilder.GetRegistry(CurrentVersion);
            var entry = wrapper?.GetGuidValue("DoesNotExist");

            entry.Should().BeNull();
        }

        [Fact]
        [SupportedOSPlatform("Windows")]
        public void GetGuidValue_WrongType()
        {
            var registryBuilder = new RegistryBuilder();

            var wrapper = registryBuilder.GetRegistry(CurrentVersion);
            var entry = wrapper?.GetGuidValue("CurrentMajorVersionNumber");

            entry.Should().BeNull();
        }

        [Fact]
        [SupportedOSPlatform("Windows")]
        public void GetGuidValue_ReturnsGuid()
        {
            var registryBuilder = new RegistryBuilder();

            var wrapper = registryBuilder.GetRegistry(CurrentVersion);
            var entry = wrapper?.GetGuidValue("BuildGUID");

            entry.Should().Be(new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));
        }

        [Fact]
        [SupportedOSPlatform("Windows")]
        public void GetValue_Dword()
        {
            var registryBuilder = new RegistryBuilder();

            var wrapper = registryBuilder.GetRegistry(CurrentVersion);
            var entry = wrapper?.GetValue("CurrentMajorVersionNumber");

            entry.Should().Contain("10");
        }

        [Fact]
        [SupportedOSPlatform("Windows")]
        public void GetValue_Binary()
        {
            var registryBuilder = new RegistryBuilder();

            var wrapper = registryBuilder.GetRegistry(CurrentVersion);
            var entry = wrapper?.GetValue("DigitalProductId");

            entry.Should().Contain(@"A40000000300000030303333312D32303330302D30303030302D414132313900F00C00005B54485D5831392D39383830340000000000000000000000000000000000000000000000B555666323AD12670300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000873E826B");
        }

        [Fact]
        [SupportedOSPlatform("Windows")]
        public void GetValue_ExpandString()
        {
            var registryBuilder = new RegistryBuilder();

            var wrapper = registryBuilder.GetRegistry(VersionKey);
            var entry = wrapper?.GetValue("ProgramFilesPath");

            entry.Should().Contain(@"C:\Program Files");
        }

        [Fact]
        [SupportedOSPlatform("Windows")]
        public void GetValue_MultiString()
        {
            var registryBuilder = new RegistryBuilder();

            var wrapper = registryBuilder.GetRegistry(@"HKLM\SOFTWARE\Microsoft\Windows\TenantRestrictions\TenantRestrictionsList");
            var entry = wrapper?.GetValue("SubdomainSupportedHostnames");

            entry.Should().Contain("{.live.com}, {.microsoft.com}, {.office.com}");
        }
    }
}