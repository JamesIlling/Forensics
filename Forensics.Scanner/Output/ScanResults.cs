﻿using Forensics.Registry.SourcedDictionary;

namespace Forensics.Scanner.Output
{
    internal class ScanResults
    {
        private const string Iso8601 = "yyyy-MM-ddTHH-mm-ss.fff";

        public List<SourcedDictionary<string, string?>> DeviceList { get; set; } = [];
        public string ComputerName { get; } = Environment.MachineName;
        public string Timestamp { get; } = DateTime.UtcNow.ToString(Iso8601);
    }
}