namespace Forensics.SetupApi.TokenClassifiers;

public class OutputText
{
    public bool Error { get; set; }
    public string? Service { get; set; }
    public string? Text { get; set; }

    public override string ToString()
    {
        return $"{Error} - {Service}: {Text}";
    }
}