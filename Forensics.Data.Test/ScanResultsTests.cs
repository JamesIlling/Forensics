using System.Globalization;
using FluentAssertions;

namespace Forensics.Data.Test;

public class ScanResultsTests
{
    [Fact]
    public void Constructor_SetsComputerName()
    {
        var scan = new ScanResults();
        scan.ComputerName.Should().Be(Environment.MachineName);
    }

    [Fact]
    public void Constructor_SetsTimeStamp()
    {
        var start = DateTime.UtcNow.AddSeconds(-1);
        var scan = new ScanResults();
        var end = DateTime.UtcNow;
        var scanTimestamp = DateTime.ParseExact(scan.Timestamp, ScanResults.Iso8601, CultureInfo.InvariantCulture,
            DateTimeStyles.None);
        scanTimestamp.Should().BeBefore(end);
        scanTimestamp.Should().BeAfter(start);
    }

    [Fact]
    public void DeviceList_IsSetDuringInit()
    {
        var deviceList = new List<SourcedDictionary<string, string?>>();
        var scan = new ScanResults
        {
            DeviceList = deviceList
        };

        scan.DeviceList.Should().AllBeEquivalentTo(deviceList);
    }

    [Fact]
    public void MountedDevice_IsSetDuringInit()
    {
        var storageList = new List<SourcedDictionary<string, string?>>();
        var scan = new ScanResults
        {
            MountedDevices = storageList
        };

        scan.MountedDevices.Should().AllBeEquivalentTo(storageList);
    }

    [Fact]
    public void StorageList_IsSetDuringInit()
    {
        var storageList = new List<SourcedDictionary<string, string?>>();
        var scan = new ScanResults
        {
            StorageList = storageList
        };

        scan.StorageList.Should().AllBeEquivalentTo(storageList);
    }
}