using Forensics.Data;

namespace Forensics.SetupApi.TokenClassifiers;

internal interface ISearchable
{
    bool ContainsUsbInfo();
    SourcedDictionary<string, string?> ToSourcedDictionary(string source);
}