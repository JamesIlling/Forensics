namespace Forensics.SetupApi.TokenClassifiers;

internal interface IClassifier
{
    TokenType TokenType { get; }
    List<Token> Tokenise(List<string> lines);
    ISearchable? Parse(List<string> lines, int start, int end);
}