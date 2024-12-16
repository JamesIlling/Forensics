using System.Drawing;
using Forensics.SetupApi.Model;
using Pastel;

namespace Forensics.SetupApi.TokenClassifiers;

internal class DeviceInstallLogClassifier : IClassifier
{
    private const string DeviceInstallLogToken = "[Device Install Log]";

    public TokenType TokenType => TokenType.DeviceInstallLog;

    private static readonly Dictionary<string, Action<string, DeviceInstallLog>> PropertiesByName = new()
    {
        { "OS Version", (text, log) => log.OperatingSystemVersion = text },
        { "Service Pack", (text, log) => log.ServicePack = text },
        { "Suite", (text, log) => log.Suite = text },
        { "ProductType", (text, log) => log.ProductType = text },
        { "Architecture", (text, log) => log.Architecture = text }
    };


    public ISearchable? Parse(List<string> lines, int start, int end)
    {
        if (!lines[start].StartsWith(DeviceInstallLogToken))
        {
            throw new NotSupportedException($"Header Missing at Line {start} ({lines[start]})");
        }

        var deviceInstallLog = new DeviceInstallLog();
        for (var line = 1; line < end; line++)
        {
            if (!string.IsNullOrWhiteSpace(lines[line]))
            {
                var parts = lines[line].Split("=", StringSplitOptions.TrimEntries);
                if (PropertiesByName.ContainsKey(parts[0]))
                {
                    PropertiesByName[parts[0]](parts[1], deviceInstallLog);
                }
                else
                {
                    throw new NotSupportedException($"Unknown Property {parts[0].Pastel(Color.Red)}");
                }
            }
        }

        return deviceInstallLog.Valid ? deviceInstallLog : null;
    }

    public List<Token> Tokenise(List<string> lines)
    {
        var tokens = new List<Token>();
        for (var lineNumber = 0; lineNumber < lines.Count; lineNumber++)
        {
            var line = lines[lineNumber];
            if (line.StartsWith(DeviceInstallLogToken))
            {
                var token = new Token
                {
                    TokenType = TokenType,
                    StartLineNumber = lineNumber,
                    EndLineNumber = lines.FindIndex(lineNumber + 1, string.IsNullOrEmpty)
                };
                tokens.Add(token);
            }
        }

        return tokens;
    }
}