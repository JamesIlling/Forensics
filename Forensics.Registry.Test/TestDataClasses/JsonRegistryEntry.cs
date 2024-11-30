using Forensics.Registry.RegistryAbstraction;
using Moq;

namespace Forensics.Registry.Test.TestDataClasses;

public class JsonRegistryEntry
{
    private const char Separator = '\\';

    public required string Name { get; set; }
    public required List<JsonRegistryEntry> SubKeys { get; set; }
    public required List<JsonRegistryValue> Values { get; set; }

    private static void BuildKey(JsonRegistryEntry key, Dictionary<string, IMock<IRegistryKey>> keys)
    {
        foreach (var child in key.SubKeys)
        {
            BuildKey(child, keys);
        }

        var mock = new Mock<IRegistryKey>();
        mock.Setup(x => x.Name).Returns(key.Name);

        var subKeyNames = key.SubKeys.Select(x => GetShortName(x.Name)).ToArray();
        mock.Setup(x => x.GetSubKeyNames()).Returns(subKeyNames);
        foreach (var name in subKeyNames)
        {
            mock.Setup(x => x.OpenSubKey(name)).Returns(keys[name].Object);
        }

        var values = key.Values.Select(x => x.Name);
        mock.Setup(x => x.GetValueNames()).Returns(values);
        foreach (var value in key.Values)
        {
            mock.Setup(x => x.GetValue(value.Name)).Returns(value.Value);
        }

        keys.Add(GetShortName(key.Name), mock);
    }

    public IMock<IRegistryBuilder> BuildMock()
    {
        var mocks = new Dictionary<string, IMock<IRegistryKey>>();
        BuildKey(this, mocks);

        var builder = new Mock<IRegistryBuilder>();
        builder.Setup(x => x.GetRegistry(It.IsAny<string>())).Returns(mocks[GetShortName(Name)].Object);
        return builder;
    }

    private static string GetShortName(string longName)
    {
        return longName.Split(Separator, StringSplitOptions.RemoveEmptyEntries)[^1];
    }
}