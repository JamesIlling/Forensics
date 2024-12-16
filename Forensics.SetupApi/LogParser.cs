using Forensics.Data;
using Forensics.SetupApi.TokenClassifiers;

namespace Forensics.SetupApi;

public class LogParser
{

    private readonly List<IClassifier> _classifiers =
    [
        new DeviceInstallLogClassifier(),
        new BeginLogClassifier(),
        new BootSessionClassifier(),
        new SectionClassifier( )
    ];

    public List<SourcedDictionary<string, string?>> Parse(List<string>? lines, string source)
    {
        var entries = new List<SourcedDictionary<string, string?>>();
        if (lines == null)
        {
            return entries;
        }

        var tokens = Tokenise(lines);


        foreach (var token in tokens)
        {
            var classifier = _classifiers.FirstOrDefault(x => x.TokenType == token.TokenType);
            var searchable = classifier?.Parse(lines, token.StartLineNumber, token.EndLineNumber);
            if (searchable != null && searchable.ContainsUsbInfo())
            {
                entries.Add(searchable.ToSourcedDictionary(source));
            }
        }
        return entries;
    }

    private List<Token> Tokenise(List<string> lines)
    {
        var tokens = new List<Token>();

        foreach (var classifier in _classifiers)
        {
            tokens.AddRange(classifier.Tokenise(lines));
        }

        return tokens;
    }
}