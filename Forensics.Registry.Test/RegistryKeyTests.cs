using System.Runtime.Versioning;
using FluentAssertions;
using Forensics.Registry.RegistryAbstraction;

namespace Forensics.Registry.Test;

[SupportedOSPlatform("Windows")]
public class RegistryKeyTests
{
    private const string VersionKey = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion";
    private const string CurrentVersion = @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion";


    [SkippableFact]
    [SupportedOSPlatform("Windows")]
    public void GetSubKeyNames()
    {
        Skip.IfNot(Environment.OSVersion.Platform == PlatformID.Win32NT);
        var registryBuilder = new RegistryBuilder();

        var wrapper = registryBuilder.GetRegistry(VersionKey);
        var entry = wrapper?.GetSubKeyNames();

        entry.Should().Contain("Uninstall");
    }

    [SkippableFact]
    [SupportedOSPlatform("Windows")]
    public void GetValue_Binary()
    {
        Skip.IfNot(Environment.OSVersion.Platform == PlatformID.Win32NT);
        var registryBuilder = new RegistryBuilder();

        var wrapper = registryBuilder.GetRegistry(CurrentVersion);
        var entry = wrapper?.GetValue("DigitalProductId");

        entry.Should().Contain(
            @"A40000000300000030303333312D32303330302D30303030302D414132313900F00C00005B54485D5831392D39383830340000000000000000000000000000000000000000000000B555666323AD12670300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000873E826B");
    }

    [SkippableFact]
    [SupportedOSPlatform("Windows")]
    public void GetValue_Dword()
    {
        Skip.IfNot(Environment.OSVersion.Platform == PlatformID.Win32NT);
        var registryBuilder = new RegistryBuilder();

        var wrapper = registryBuilder.GetRegistry(CurrentVersion);
        var entry = wrapper?.GetValue("CurrentMajorVersionNumber");

        entry.Should().Contain("10");
    }

    [SkippableFact]
    [SupportedOSPlatform("Windows")]
    public void GetValue_ExpandString()
    {
        Skip.IfNot(Environment.OSVersion.Platform == PlatformID.Win32NT);
        var registryBuilder = new RegistryBuilder();

        var wrapper = registryBuilder.GetRegistry(VersionKey);
        var entry = wrapper?.GetValue("ProgramFilesPath");

        entry.Should().Contain(@"C:\Program Files");
    }

    [SkippableFact]
    [SupportedOSPlatform("Windows")]
    public void GetValue_MultiString()
    {
        Skip.IfNot(Environment.OSVersion.Platform == PlatformID.Win32NT);
        var registryBuilder = new RegistryBuilder();

        var wrapper =
            registryBuilder.GetRegistry(@"HKLM\SOFTWARE\Microsoft\Windows\TenantRestrictions\TenantRestrictionsList");
        var entry = wrapper?.GetValue("SubdomainSupportedHostnames");

        entry.Should().Contain("{.live.com}, {.microsoft.com}, {.office.com}");
    }


    [SkippableFact]
    [SupportedOSPlatform("Windows")]
    public void GetValue_String()
    {
        Skip.IfNot(Environment.OSVersion.Platform == PlatformID.Win32NT);
        var registryBuilder = new RegistryBuilder();

        var wrapper = registryBuilder.GetRegistry(CurrentVersion);
        var entry = wrapper?.GetValue("SystemRoot");

        entry.Should().Contain(@"C:\WINDOWS");
    }

    [SkippableFact]
    [SupportedOSPlatform("Windows")]
    public void GetValue_UnknownValue()
    {
        Skip.IfNot(Environment.OSVersion.Platform == PlatformID.Win32NT);
        var registryBuilder = new RegistryBuilder();

        var wrapper = registryBuilder.GetRegistry(CurrentVersion);
        var entry = wrapper?.GetValue("DoesNotExist");

        entry.Should().BeNull();
    }

    [SkippableFact]
    [SupportedOSPlatform("Windows")]
    public void GetValueNames_ReturnNamesOfValuesForTheKey()
    {
        Skip.IfNot(Environment.OSVersion.Platform == PlatformID.Win32NT);
        var registryBuilder = new RegistryBuilder();

        var wrapper = registryBuilder.GetRegistry(VersionKey);
        var entry = wrapper?.GetValueNames();

        entry.Should().Contain("ProgramFilesPath");
    }


    [SkippableFact]
    [SupportedOSPlatform("Windows")]
    public void OpenSubKey_OpensKey()
    {
        Skip.IfNot(Environment.OSVersion.Platform == PlatformID.Win32NT);
        var registryBuilder = new RegistryBuilder();

        var wrapper = registryBuilder.GetRegistry(VersionKey);
        var entry = wrapper?.OpenSubKey("Uninstall");
        entry.Should().NotBeNull();
    }


    [SkippableFact]
    [SupportedOSPlatform("Windows")]
    public void OpenSubKey_ShouldBeNull_IfKeyDoesNotExist()
    {
        Skip.IfNot(Environment.OSVersion.Platform == PlatformID.Win32NT);
        var registryBuilder = new RegistryBuilder();

        var wrapper = registryBuilder.GetRegistry(VersionKey);
        var entry = wrapper?.OpenSubKey("NOPE");
        entry.Should().BeNull();
    }
}