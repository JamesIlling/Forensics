using System.Collections;
using System.Text.Json;

namespace Forensics.Registry.Test.TestDataClasses;

public class RegistryJsonFileReader : IEnumerable<object[]>
{
    private readonly string _path;

    public RegistryJsonFileReader(string path, int column)
    {
        _path = path;
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        var json = JsonSerializer.Deserialize<RegistryJsonTestFile>(File.ReadAllText(_path));

        foreach (var value in json?.ExpectedValues ?? [])
        {
            yield return [json!.Registry.BuildMock(), value.Name, value.Value];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}