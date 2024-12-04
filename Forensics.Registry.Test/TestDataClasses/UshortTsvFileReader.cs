using System.Collections;
using System.Globalization;

namespace Forensics.Registry.Test.TestDataClasses;

public class UshortTsvFileReader : IEnumerable<object[]>
{
    private readonly int _column;
    private readonly string _path;

    public UshortTsvFileReader(string path, int column)
    {
        _path = path;
        _column = column;
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        var lines = File.ReadAllLines(_path);
        foreach (var item in lines)
        {
            var parts = item.Split('\t');
            if (parts.Length > _column)
            {
                var part = parts[_column];
                if (!string.IsNullOrEmpty(part))
                {
                    var data = ushort.Parse(part, NumberStyles.HexNumber);
                    yield return [parts[0], data];
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}