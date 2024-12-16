using Forensics.Data;
using Forensics.Registry.Scanners;
using System.Drawing;
using Pastel;

namespace Forensics.SetupApi
{
    public class SetupApiScanner : IScan<SourcedDictionary<string, string?>>
    {
        private const string LogFileFolderPath = @"C:\windows\inf\";
        private const string LogFileFilter = "setupapi.dev*.log";

        public string Name => nameof(SetupApiScanner);

        public List<SourcedDictionary<string, string?>> Scan()
        {
            var entries = new List<SourcedDictionary<string, string?>>();
            var parser = new LogParser();
            var files = GetSetupLogs(LogFileFolderPath, LogFileFilter).ToList();
            foreach (var file in files)
            {
                Console.WriteLine($"  Scanning {file}".Pastel(Color.DarkGreen));
                var lines = File.ReadAllLines(file).ToList();
                entries.AddRange(parser.Parse(lines, file));

            }
            return entries.Distinct().ToList();
        }

        private static IEnumerable<string> GetSetupLogs(string path, string filter)
        {
            var di = new DirectoryInfo(path);
            return di.GetFiles(filter).Select(x => x.FullName);
        }
    }
}
