using FluentAssertions;
using Forensics.Registry.Test.TestDataClasses;

namespace Forensics.Registry.Test
{
    public class StringExtensionTests
    {

        [Fact]

        public void ParseVendorId_ReturnsNull_WhenNoVidPresent()
        {
            "NoVid".ParseVendorId().Should().BeNull();
        }


        [Fact]

        public void ParseVendorId_ReturnsNull_WhenNullProvided()
        {
            StringExtensions.ParseVendorId(null).Should().BeNull();
        }

        [Theory]
        [FileData(typeof(UshortTsvFileReader), "Resources/HardwareIdTestData.tsv")]
        public void ParseVendorId_ReturnsId_WhenPresent(string hardwareId, ushort? expectedValue)
        {
            hardwareId.ParseVendorId().Should().Be(expectedValue);
        }

        [Fact]

        public void ParseProductId_ReturnsNull_WhenNoPidPresent()
        {
            "NoPid".ParseProductId().Should().BeNull();
        }


        [Fact]

        public void ParseProductId_ReturnsNull_WhenNullProvided()
        {
            StringExtensions.ParseProductId(null).Should().BeNull();
        }

        [Theory]
        [FileData(typeof(UshortTsvFileReader), "Resources/HardwareIdTestData.tsv", 2)]
        public void ParseProductId_ReturnsId_WhenPresent(string hardwareId, ushort? expectedValue)
        {
            hardwareId.ParseProductId().Should().Be(expectedValue);
        }

        [Fact]

        public void ParseRevisionId_ReturnsNull_WhenNoRevisionPresent()
        {
            "NoRev".ParseRevision().Should().BeNull();
        }


        [Fact]

        public void ParseRevision_ReturnsNull_WhenNullProvided()
        {
            StringExtensions.ParseRevision(null).Should().BeNull();
        }

        [Theory]
        [FileData(typeof(UshortTsvFileReader), "Resources/HardwareIdTestData.tsv", 3)]
        public void ParseRevision_ReturnsId_WhenPresent(string hardwareId, ushort? expectedValue)
        {
            hardwareId.ParseRevision().Should().Be(expectedValue);
        }

        [Fact]

        public void ParseInterface_ReturnsNull_WhenNoRevisionPresent()
        {
            "NoMi".ParseInterface().Should().BeNull();
        }


        [Fact]

        public void ParseInterface_ReturnsNull_WhenNullProvided()
        {
            StringExtensions.ParseInterface(null).Should().BeNull();
        }

        [Theory]
        [FileData(typeof(UshortTsvFileReader), "Resources/HardwareIdTestData.tsv", 4)]
        public void ParseInterface_ReturnsId_WhenPresent(string hardwareId, ushort? expectedValue)
        {
            Convert.ToUInt16(hardwareId.ParseInterface()).Should().Be(expectedValue);
        }
    }
}