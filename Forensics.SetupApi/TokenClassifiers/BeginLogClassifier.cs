namespace Forensics.SetupApi.TokenClassifiers;

internal class BeginLogClassifier : IClassifier
{
    private const string BeginLogToken = "[BeginLog]";

    public TokenType TokenType => TokenType.BeginLog;

    public List<Token> Tokenise(List<string> lines)
    {
        var tokens = new List<Token>();
        for (var lineNumber = 0; lineNumber < lines.Count; lineNumber++)
        {
            var line = lines[lineNumber];
            if (line.StartsWith(BeginLogToken))
            {
                var token = new Token
                {
                    TokenType = TokenType,
                    StartLineNumber = lineNumber,
                    EndLineNumber = lines.Count - 1
                };
                tokens.Add(token);
            }
        }

        return tokens;
    }

    public ISearchable? Parse(List<string> lines, int start, int end)
    {
        // This is just a section header with the BeginLogToken there is no data, so we shall return null
        return null;
    }
}