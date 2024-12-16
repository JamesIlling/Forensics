using System.Globalization;

namespace Forensics.SetupApi.TokenClassifiers;

internal class SectionClassifier : IClassifier
{
    private const string SectionStartToken = ">>>  [";
    private const string SectionEndToken = "<<<  [Exit status: ";
    private const string TimestampStartToken = ">>>  Section start ";
    private const string TimestampEndToken = "<<<  Section end ";
    private const string CommandToken = "      cmd: ";

    public TokenType TokenType => TokenType.Section;

    public List<Token> Tokenise(List<string> lines)
    {
        var tokens = new List<Token>();
        for (var lineNumber = 0; lineNumber < lines.Count; lineNumber++)
        {
            var line = lines[lineNumber];
            if (line.StartsWith(SectionStartToken))
            {
                var token = new Token
                {
                    TokenType = TokenType,
                    StartLineNumber = lineNumber,
                    EndLineNumber = lines.FindIndex(lineNumber + 1, x => x.StartsWith(SectionEndToken))
                };
                tokens.Add(token);
            }
        }

        return tokens;
    }

    public ISearchable? Parse(List<string> lines, int start, int end)
    {
        var section = new Section();
        var current = start;
        section.Action = lines[start].Replace(SectionStartToken, "")[..^1];
        current++;
        var startTimestampString = lines[current].Replace(TimestampStartToken, "");
        section.StartTimestamp = DateTime.ParseExact(startTimestampString, "yyyy/MM/dd HH:mm:ss.fff",
            CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        current++;
        section.Command = lines[current].Replace(CommandToken, "");
        current++;
        OutputText? output = new OutputText();
        while (!lines[current].StartsWith(TimestampEndToken, StringComparison.InvariantCultureIgnoreCase) && output != null)
        {
            output = ParseOutputText(lines, current);
            if (output != null)
            {
                section.Output.Add(output);
            }

            current++;
        }

        if (output != null)
        {
            var endTimestampString = lines[current].Replace(TimestampEndToken, "");
            section.EndTimestamp = DateTime.ParseExact(endTimestampString, "yyyy/MM/dd HH:mm:ss.fff",
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            current++;
            section.Status = lines[current].Replace(SectionEndToken, "")[..^1];
        }

        return section;
    }

    private OutputText? ParseOutputText(List<string> lines, int current)
    {
        var line = lines[current];

        if (line.Length > 9)
        {
            var outputText = new OutputText
            {
                Error = lines[current][0] == '!',
                Service = lines[current][6..8],
                Text = lines[current][9..].Trim()
            };
            return outputText;
        }
        else
        {
            Console.WriteLine($"Issue when parsing Output text({line}) - {current} ");
        }

        return null;

    }
}
