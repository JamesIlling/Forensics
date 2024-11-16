namespace Forensics.Registry.SourcedDictionary;

public record SourcedKeyValuePair<TKey, TValue>
{
    public TKey? Key { get; init; }
    public TValue? Value { get; init; }
    public string? Source { get; init; }

}