namespace Forensics.Data
{
    public class ScanResults
    {
        public const string Iso8601 = "yyyy-MM-ddTHH-mm-ss.fff";

        public List<SourcedDictionary<string, string?>> DeviceList { get; init; } = [];
        public List<SourcedDictionary<string, string?>> StorageList { get; init; } = [];
        public string ComputerName { get; } = Environment.MachineName;
        public string Timestamp { get; } = DateTime.UtcNow.ToString(Iso8601);
    }
}
