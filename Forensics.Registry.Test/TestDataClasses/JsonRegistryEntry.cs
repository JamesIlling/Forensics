using System.ComponentModel.DataAnnotations;
using Forensics.Registry.RegistryAbstraction;
using Moq;

namespace Forensics.Registry.Test.TestDataClasses
{
    public class JsonRegistryEntry
    {

        public string Name { get; set; }
        public List<JsonRegistryEntry> SubKeys { get; set; }
        public List<JsonRegistryValue> Values { get; set; }



        private static void BuildKey(JsonRegistryEntry key, Dictionary<string, IMock<IRegistryKey>> keys)
        {
            foreach (var child in key.SubKeys)
            {
                BuildKey(child, keys);
            }


            var mock = new Mock<IRegistryKey>();
            mock.Setup(x => x.Name).Returns(key.Name);

            var subkeyNames = key.SubKeys.Select(x => x.Name.Split('\\', StringSplitOptions.RemoveEmptyEntries).Last()).ToArray();
            mock.Setup(x => x.GetSubKeyNames()).Returns(subkeyNames);
            foreach (var name in subkeyNames)
            {
                mock.Setup(x => x.OpenSubKey(name)).Returns(keys[name].Object);
            }

            var values = key.Values.Select(x => x.Name);
            mock.Setup(x => x.GetValueNames()).Returns(values);
            foreach (var value in key.Values)
            {
                mock.Setup(x => x.GetValue(value.Name)).Returns(value.Value);
            }
            keys.Add(key.Name.Split('\\', StringSplitOptions.RemoveEmptyEntries).Last(), mock);
        }

        public IMock<IRegistryBuilder> BuildMock()
        {
            var mocks = new Dictionary<string, IMock<IRegistryKey>>();
            BuildKey(this, mocks);

            var builder = new Mock<IRegistryBuilder>();
            builder.Setup(x => x.GetRegistry(It.IsAny<string>())).Returns(mocks[Name.Split('\\', StringSplitOptions.RemoveEmptyEntries).Last()].Object);
            return builder;
        }
    }
}

