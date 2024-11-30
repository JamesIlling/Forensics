using System.Text;
using Forensics.Data;

namespace Forensics.Scanner.Output;

internal class TestTsvOutput : IOutput
{
    public void Output(ScanResults data)
    {
        var lines = new List<string>();
        foreach (var device in data.DeviceList)
        {
            var line = new StringBuilder();
            line.Append(device.FirstOrDefault(x => x.Key == "HardwareID")?.Value);
            line.Append('\t');
            line.Append(device.FirstOrDefault(x => x.Key == "VendorId")?.Value);
            line.Append('\t');
            line.Append(device.FirstOrDefault(x => x.Key == "ProductId")?.Value);
            line.Append('\t');
            line.Append(device.FirstOrDefault(x => x.Key == "Revision")?.Value);
            line.Append('\t');
            line.Append(device.FirstOrDefault(x => x.Key == "InterfaceId")?.Value);
            line.AppendLine();
            lines.Add(line.ToString());
        }

        File.WriteAllLines("HardwareIdTestData.tsv", lines);
    }
}