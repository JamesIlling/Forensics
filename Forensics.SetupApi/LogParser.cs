using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection.Emit;
using Forensics.Data;
using Forensics.SetupApi.TokenClassifiers;
using Pastel;

namespace Forensics.SetupApi;

public class LogParser
{

    private static readonly List<IClassifier> Classifiers =
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
            var classifier = Classifiers.FirstOrDefault(x => x.TokenType == token.TokenType);
            var searchable = classifier?.Parse(lines, token.StartLineNumber, token.EndLineNumber);
            if (searchable != null && searchable.ContainsUsbInfo())
            {
                entries.Add(searchable.ToSourcedDictionary(source));
            }
        }
        return entries;
    }

    private static List<Token> Tokenise(List<string> lines)
    {
        var tokens = new List<Token>();

        foreach (var classifier in Classifiers)
        {
            tokens.AddRange(classifier.Tokenise(lines));
        }

        return tokens;
    }
}