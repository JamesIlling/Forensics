using System.Globalization;
using Forensics.Data;

namespace Forensics.SetupApi.TokenClassifiers;

internal class BootSessionClassifier : IClassifier
{
    private const string BootSessionToken = "[Boot Session: ";

    public TokenType TokenType => TokenType.BootSession;

    public List<Token> Tokenise(List<string> lines)
    {
        var tokens = new List<Token>();
        for (var lineNumber = 0; lineNumber < lines.Count; lineNumber++)
        {
            var line = lines[lineNumber];
            if (line.StartsWith(BootSessionToken))
            {
                var next = lines.FindIndex(lineNumber + 1, x => x.StartsWith(BootSessionToken));

                var token = new Token
                {
                    TokenType = TokenType,
                    StartLineNumber = lineNumber,
                    EndLineNumber = next == -1 ? lines.Count - 1 : next
                };
                tokens.Add(token);
            }
        }

        return tokens;
    }

    public ISearchable? Parse(List<string> lines, int start, int end)
    {
        if (lines[start].Length > 38)
        {
            var date = lines[start].Replace(BootSessionToken, "")[..^1];
            return new BootSession()
            {
                Timestamp = DateTime.ParseExact(date, "yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal)
            };
        }
        return null;
    }
}

public record BootSession : ISearchable
{
    private const string Iso8601 = "yyyy-MM-ddTHH-mm-ss.fff";

    public DateTime Timestamp { get; set; }

    public bool ContainsUsbInfo()
    {
        return false;
    }

    public SourcedDictionary<string, string?> ToSourcedDictionary(string source)
    {
        return new SourcedDictionary<string, string?> { { source, "Timestamp", Timestamp.ToString(Iso8601) } };
    }
}