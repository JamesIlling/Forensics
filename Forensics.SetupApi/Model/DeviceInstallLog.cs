using Forensics.Data;
using Forensics.SetupApi.TokenClassifiers;

namespace Forensics.SetupApi.Model;

internal record DeviceInstallLog : ISearchable
{
    public string? OperatingSystemVersion { get; set; }

    public bool Valid => OperatingSystemVersion != null
                         && ServicePack != null
                         && Suite != null
                         && ProductType != null
                         && Architecture != null;

    public string? ServicePack { get; set; }
    public string? Suite { get; set; }
    public string? ProductType { get; set; }
    public string? Architecture { get; set; }

    public bool ContainsUsbInfo()
    {
        return false;
    }

    public SourcedDictionary<string, string?> ToSourcedDictionary(string source)
    {
        var entry = new SourcedDictionary<string, string?>();
        foreach (var property in typeof(DeviceInstallLog).GetProperties().Where(x => x.CanWrite))
        {
            entry.Add(source, property.Name, property.GetValue(this)?.ToString());
        }

        return entry;
    }
}

