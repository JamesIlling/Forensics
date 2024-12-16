namespace Forensics.Data;

public class ScanResults
{
    public const string Iso8601 = "yyyy-MM-ddTHH-mm-ss.fff";
    public string ComputerName { get; } = Environment.MachineName;

    public List<SourcedDictionary<string, string?>> DeviceList { get; init; } = [];
    public List<SourcedDictionary<string, string?>> MountedDevices { get; init; } = [];
    public List<SourcedDictionary<string, string?>> StorageList { get; init; } = [];
    public List<SourcedDictionary<string, string?>> CurrentlyAttachedDevices { get; init; } = [];
    public string Timestamp { get; } = DateTime.UtcNow.ToString(Iso8601);
    public List<SourcedDictionary<string, string?>> SetupLogs { get; set; } = [];
}