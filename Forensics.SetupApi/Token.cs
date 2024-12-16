namespace Forensics.SetupApi;

public record Token
{

    public TokenType TokenType { get; set; }
    public int EndLineNumber { get; set; }
    public int StartLineNumber { get; set; }
}