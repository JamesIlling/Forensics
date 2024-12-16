using System.Drawing;
using Forensics.Data;
using Pastel;

namespace Forensics.Scanner.Output;

internal class ConsoleDisplay : IOutput
{
    private static readonly Color SourceColour = Color.DimGray;
    private static readonly Color PropertyNameColour = Color.DarkOliveGreen;
    private static readonly Color StoragePropertyNameColour = Color.SteelBlue;
    private static readonly Color MountedDevicePropertyNameColour = Color.SlateBlue;
    private static readonly Color SetupLogsColour = Color.Purple;
    private static readonly Color CurrentlyAttachedColour = Color.Coral;
    private static readonly Color PropertyValueColour = Color.LightGray;
    private static readonly Color ScanDetailsColor = Color.DarkGoldenrod;

    public void Output(ScanResults data)
    {
        Console.WriteLine($"Scan {data.ComputerName} at {data.Timestamp}".Pastel(ScanDetailsColor));

        Console.WriteLine("Currently Attached".Pastel(ScanDetailsColor));
        foreach (var device in data.CurrentlyAttachedDevices)
        {
            foreach (var property in device.OrderBy(x => x.Key))
            {
                Console.WriteLine(
                    $"{property.Key.Pastel(CurrentlyAttachedColour)}: {property.Value.Pastel(PropertyValueColour)} {property.Source.Pastel(SourceColour)}");
            }

            Console.WriteLine();
        }

        Console.WriteLine("Devices".Pastel(ScanDetailsColor));
        foreach (var device in data.DeviceList)
        {
            foreach (var property in device.OrderBy(x => x.Key))
            {
                Console.WriteLine(
                    $"{property.Key.Pastel(PropertyNameColour)}: {property.Value.Pastel(PropertyValueColour)} {property.Source.Pastel(SourceColour)}");
            }

            Console.WriteLine();
        }

        Console.WriteLine("Storage".Pastel(ScanDetailsColor));
        foreach (var volume in data.StorageList)
        {
            foreach (var property in volume.OrderBy(x => x.Key))
            {
                Console.WriteLine(
                    $"{property.Key.Pastel(StoragePropertyNameColour)}: {property.Value.Pastel(PropertyValueColour)} {property.Source.Pastel(SourceColour)}");
            }

            Console.WriteLine();
        }

        Console.WriteLine("MountedDevices".Pastel(ScanDetailsColor));
        foreach (var volume in data.MountedDevices)
        {
            foreach (var property in volume.OrderBy(x => x.Key))
            {
                Console.WriteLine(
                    $"{property.Key.Pastel(MountedDevicePropertyNameColour)}: {property.Value.Pastel(PropertyValueColour)} {property.Source.Pastel(SourceColour)}");
            }

            Console.WriteLine();
        }

        Console.WriteLine("SetupLog".Pastel(ScanDetailsColor));
        foreach (var volume in data.SetupLogs)
        {
            foreach (var property in volume.OrderBy(x => x.Key))
            {
                if (property.Value?.Length < 1000)
                {
                    Console.WriteLine(
                        $"{property.Key.Pastel(SetupLogsColour)}: {property.Value.Pastel(PropertyValueColour)} {property.Source.Pastel(SourceColour)}");
                }
                else
                {
                    Console.WriteLine(
                        $"{property.Key.Pastel(SetupLogsColour)}: <REMOVED> {property.Source.Pastel(SourceColour)}");
                }
            }

            Console.WriteLine();
        }
    }
}