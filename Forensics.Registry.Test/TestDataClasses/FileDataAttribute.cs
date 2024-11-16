using System.Globalization;
using System.Reflection;
using Xunit.Sdk;

namespace Forensics.Registry.Test.TestDataClasses;


[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class FileDataAttribute : DataAttribute
{

    public FileDataAttribute(Type @class, string filePath, int column = 1)
    {
        Class = @class;
        Path = filePath;
        Column = column;
    }

    public Type Class { get; }
    public string Path { get; }
    public int Column { get; }


    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        if (Activator.CreateInstance(Class, [Path, Column]) is not IEnumerable<object[]> data)
            throw new ArgumentException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    "{0} must implement IEnumerable<object[]> to be used as ClassData for the test method named '{1}' on {2}",
                    Class.FullName,
                    testMethod.Name,
                    testMethod.DeclaringType?.FullName
                )
            );

        return data;
    }
}