using System.Drawing;
using Forensics.Data;
using Pastel;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    private static void Display(List<SourcedDictionary<string, string?>> dataset, Color propertyColour)
    {
        foreach (var device in dataset)
        {
            foreach (var property in device.OrderBy(x => x.Key))
            {
                Console.WriteLine(
                    $"{property.Key.Pastel(propertyColour)}: {property.Value.Pastel(PropertyValueColour)} {property.Source.Pastel(SourceColour)}");
            }

            Console.WriteLine();
        }
    }
    private static void Display(List<SourcedDictionary<string, string?>> dataset, Color propertyColour, int maxLength)
    {
        foreach (var volume in dataset)
        {
            foreach (var property in volume.OrderBy(x => x.Key))
            {
                if (property.Value?.Length < maxLength)
                {
                    Console.WriteLine(
                        $"{property.Key.Pastel(propertyColour)}: {property.Value.Pastel(PropertyValueColour)} {property.Source.Pastel(SourceColour)}");
                }
                else
                {
                    Console.WriteLine(
                        $"{property.Key.Pastel(propertyColour)}: <REMOVED> {property.Source.Pastel(SourceColour)}");
                }
            }

            Console.WriteLine();
        }
    }


    public void Output(ScanResults data)
    {
        Console.WriteLine($"Scan {data.ComputerName} at {data.Timestamp}".Pastel(ScanDetailsColor));

        Console.WriteLine("Currently Attached".Pastel(ScanDetailsColor));
        Display(data.CurrentlyAttachedDevices, CurrentlyAttachedColour);

        Console.WriteLine("Devices".Pastel(ScanDetailsColor));
        Display(data.DeviceList, PropertyNameColour);


        Console.WriteLine("Storage".Pastel(ScanDetailsColor));
        Display(data.StorageList, StoragePropertyNameColour);

        Console.WriteLine("MountedDevices".Pastel(ScanDetailsColor));
        Display(data.MountedDevices, MountedDevicePropertyNameColour);

        Console.WriteLine("SetupLog".Pastel(ScanDetailsColor));
        Display(data.SetupLogs, SetupLogsColour, 1000);
    }
}