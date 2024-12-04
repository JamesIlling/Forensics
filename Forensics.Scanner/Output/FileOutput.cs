using System.Text;
using Forensics.Data;

namespace Forensics.Scanner.Output;

public class FileOutput : IOutput
{
    public void Output(ScanResults data)
    {
        var text = new StringBuilder();
        text.AppendLine($"Scan {data.ComputerName} at {data.Timestamp}");
        text.AppendLine("Devices");
        foreach (var device in data.DeviceList)
        {
            foreach (var property in device.OrderBy(x => x.Key))
            {
                text.AppendLine($"{property.Key}: {property.Value} {property.Source}");
            }

            text.AppendLine();
        }

        text.AppendLine("Storage");

        foreach (var volume in data.StorageList)
        {
            foreach (var property in volume.OrderBy(x => x.Key))
            {
                text.AppendLine(
                    $"{property.Key}: {property.Value} {property.Source}");
            }

            text.AppendLine();
        }

        text.AppendLine("MountedDevices");
        foreach (var volume in data.MountedDevices)
        {
            foreach (var property in volume.OrderBy(x => x.Key))
            {
                text.AppendLine($"{property.Key}: {property.Value} {property.Source}");
            }

            text.AppendLine();
        }

        File.WriteAllText("items.txt", text.ToString());
    }
}