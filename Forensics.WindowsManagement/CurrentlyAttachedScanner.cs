using System.Management;
using System.Runtime.Versioning;
using Forensics.Data;
using Forensics.Registry.Scanners;

namespace Forensics.WindowsManagement;

[SupportedOSPlatform("Windows")]
public class CurrentlyAttachedScanner : IScan<SourcedDictionary<string, string?>>
{
    private const string Source = "Win32_PnPEntity";

    /// <summary>
    /// Query to pull all USB devices connected to this PC currently according to PlugNPlay.
    /// </summary>
    private const string Query = "SELECT * FROM Win32_PnPEntity where DeviceID Like \"USB%\"";

    public string Name => nameof(CurrentlyAttachedScanner);

    public List<SourcedDictionary<string, string?>> Scan()
    {
        var results = new List<SourcedDictionary<string, string?>>();

        using var searcher = new ManagementObjectSearcher(Query);
        foreach (var device in searcher.Get())
        {
            var item = new SourcedDictionary<string, string?>();
            foreach (var property in device.Properties)
            {
                if (property.Value != null)
                {
                    item.Add(Source, property.Name, property.Value.ToString());
                }
            }

            results.Add(item);
        }

        return results;
    }
}