using Forensics.Data;

namespace Forensics.SetupApi.TokenClassifiers;

public record Section : ISearchable
{
    private const string Iso8601 = "yyyy-MM-ddTHH-mm-ss.fff";
    private const string UsbStorageService = "USBSTOR";
    private const string Usb = "USB\\";

    public string? Action { get; set; }
    public DateTime? StartTimestamp { get; set; }
    public string? Command { get; set; }

    public List<OutputText> Output { get; set; } = [];
    public DateTime? EndTimestamp { get; set; }
    public string? Status { get; set; }

    public bool ContainsUsbInfo()
    {
        return Output.Any(x => x.Text?.Contains(UsbStorageService, StringComparison.OrdinalIgnoreCase) ?? false)
               || Output.Any(x => x.Text?.Contains(Usb, StringComparison.OrdinalIgnoreCase) ?? false)
               || (Action?.Contains(UsbStorageService, StringComparison.OrdinalIgnoreCase) ?? false)
               || (Action?.Contains(Usb, StringComparison.OrdinalIgnoreCase) ?? false)
               || (Command?.Contains(UsbStorageService, StringComparison.OrdinalIgnoreCase) ?? false)
               || (Command?.Contains(Usb, StringComparison.OrdinalIgnoreCase) ?? false);

    }

    public SourcedDictionary<string, string?> ToSourcedDictionary(string source)
    {
        return new SourcedDictionary<string, string?>
        {
            { source, "Action", Action },
            { source, "Start", StartTimestamp?.ToString(Iso8601) },
            { source, "Command", Command },
            { source, "Output", string.Join("\r\n", Output.Select(x => x.ToString())) },
            { source, "End", EndTimestamp?.ToString(Iso8601) },
            { source, "Status", Status }
        };

    }
}