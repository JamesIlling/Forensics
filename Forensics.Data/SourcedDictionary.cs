using System.Collections;

namespace Forensics.Data;

public class SourcedDictionary<TKey, TValue> : ICollection<SourcedKeyValuePair<TKey, TValue>> where TValue : class?
{
    private readonly List<SourcedKeyValuePair<TKey, TValue>> _data = [];
    public Guid? Id { get; set; }

    public IEnumerator<SourcedKeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public TValue? Get(TKey key)
    {
        return _data.Find(x => Equals(x.Key, key))?.Value;
    }

    public void Add(string source, TKey key, TValue? value)
    {
        var item = new SourcedKeyValuePair<TKey, TValue> { Key = key, Source = source, Value = value };
        Add(item);
    }

    public void Add(SourcedKeyValuePair<TKey, TValue> item)
    {
        if (item.Source == null || item.Key is null || Equals(item.Value, null))
        {
            return;
        }


        if (typeof(TValue) == typeof(string))
        {
            if (string.IsNullOrWhiteSpace(item.Value.ToString()))
            {
                return;
            }

            item = item with { Value = item.Value.ToString()?.Trim() as TValue };
        }

        var existing = _data.Find(x => x.Source == item.Source && x.Key!.Equals(item.Key));

        if (existing != null)
        {
            _data.Remove(existing);
        }
        _data.Add(item);
    }

    public void Clear()
    {
        _data.Clear();
    }

    public bool Contains(SourcedKeyValuePair<TKey, TValue> item)
    {
        return _data.Contains(item);
    }

    public void CopyTo(SourcedKeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        _data.CopyTo(array, arrayIndex);
    }


    public bool Remove(SourcedKeyValuePair<TKey, TValue> item)
    {
        return _data.Remove(item);
    }

    public int Count => _data.Count;
    public bool IsReadOnly => false;
}