namespace Forensics.Data;

public record SourcedKeyValuePair<TKey, TValue>
{
    public TKey? Key { get; init; }
    public string? Source { get; init; }
    public TValue? Value { get; init; }
}