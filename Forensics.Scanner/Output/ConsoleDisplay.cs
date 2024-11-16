using System.Drawing;
using Pastel;

namespace Forensics.Scanner.Output
{
    internal class ConsoleDisplay : IOutput
    {
        private static readonly Color SourceColour = Color.DimGray;
        private static readonly Color PropertyNameColour = Color.DarkOliveGreen;
        private static readonly Color PropertyValueColour = Color.LightGray;
        private static readonly Color ScanDetailsColor = Color.DarkGoldenrod;

        public void Output(ScanResults data)
        {
            Console.WriteLine($"Scan {data.ComputerName} at {data.Timestamp}".Pastel(ScanDetailsColor));

            foreach (var device in data.DeviceList)
            {
                foreach (var property in device)
                {
                    Console.WriteLine($"{property.Key.Pastel(PropertyNameColour)}: {property.Value.Pastel(PropertyValueColour)} {property.Source.Pastel(SourceColour)}");
                }
                Console.WriteLine();
            }
        }
    }
}
