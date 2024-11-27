using Forensics.Data;
using Forensics.Registry.Scanners;
using Forensics.Scanner.Output;

namespace Forensics.Scanner
{
    internal class UsbScanner
    {
        private readonly List<IScan<SourcedDictionary<string, string?>>> _scanners;

        public UsbScanner(IEnumerable<IScan<SourcedDictionary<string, string?>>> scanners)
        {
            _scanners = scanners.ToList();
        }

        public ScanResults Scan()
        {
            var scannerResults = new Dictionary<string, List<SourcedDictionary<string, string?>>>();
            foreach (var scanner in _scanners)
            {
                var items = scanner.Scan();
                scannerResults.Add(scanner.Name, items);
            }

            var results = new ScanResults { DeviceList = scannerResults["UsbEnumerationScanner"], StorageList = scannerResults["UsbStorageEnumerationScanner"] };
            return results;
        }
    }
}
