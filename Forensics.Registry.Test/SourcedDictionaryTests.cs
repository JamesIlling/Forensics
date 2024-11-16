using FluentAssertions;
using Forensics.Registry.SourcedDictionary;

namespace Forensics.Registry.Test
{
    public class SourcedDictionaryTests
    {
        [Fact]
        public void Implements_ICollection()
        {
            typeof(SourcedDictionary<string, string>).Should().BeAssignableTo(typeof(ICollection<SourcedKeyValuePair<string, string>>));
        }

        [Fact]
        public void IsReadOnly_isFalse()
        {
            var sourcedDictionary = new SourcedDictionary<string, string>();
            sourcedDictionary.IsReadOnly.Should().BeFalse();
        }

        [Fact]
        public void Add_DoesNotAdd_WhenValueIsNull()
        {
            var sourcedDictionary = new SourcedDictionary<string, string> { { "a", "b", null } };
            sourcedDictionary.Count.Should().Be(0);
        }

        [Fact]
        public void Add_DoesNotAdd_WhenValueIsNull_KeyValuePair()
        {
            var sourcedDictionary = new SourcedDictionary<string, string>();
            var data = new SourcedKeyValuePair<string, string>()
            {
                Key = "a",
                Source = "b"

            };
            sourcedDictionary.Add(data);
            sourcedDictionary.Count.Should().Be(0);
        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        [Fact]
        public void Add_DoesNotAdd_WhenSourceIsNull()
        {
            var sourcedDictionary = new SourcedDictionary<string, string> { { null, "b", "c" } };
            sourcedDictionary.Count.Should().Be(0);
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        [Fact]
        public void Add_DoesNotAdd_WhenSourceIsNull_KeyValuePair()
        {
            var sourcedDictionary = new SourcedDictionary<string, string>();
            var data = new SourcedKeyValuePair<string, string>()
            {
                Key = "a",
                Value = "c"

            };
            sourcedDictionary.Add(data);
            sourcedDictionary.Count.Should().Be(0);
        }

        [Fact]
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        public void Add_DoesNotAdd_WhenKeyIsNull()
        {

            var sourcedDictionary = new SourcedDictionary<string, string> { { "a", null, "c" } };
            sourcedDictionary.Count.Should().Be(0);
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        [Fact]
        public void Add_DoesNotAdd_WhenKeyIsNull_KeyValuePair()
        {
            var sourcedDictionary = new SourcedDictionary<string, string>();
            var data = new SourcedKeyValuePair<string, string>()
            {
                Source = "b",
                Value = "c"

            };
            sourcedDictionary.Add(data);
            sourcedDictionary.Count.Should().Be(0);
        }

        [Fact]
        public void Add_DoesNotAdd_WhenValueIsWhitespace()
        {
            var sourcedDictionary = new SourcedDictionary<string, string> { { "a", "b", " " } };
            sourcedDictionary.Count.Should().Be(0);
        }

        [Fact]
        public void Add_DoesNotAdd_WhenValueIsWhitespace_KeyValuePair()
        {
            var sourcedDictionary = new SourcedDictionary<string, string>();
            var data = new SourcedKeyValuePair<string, string>()
            {
                Key = "a",
                Source = "b",
                Value = " "

            };
            sourcedDictionary.Add(data);
            sourcedDictionary.Count.Should().Be(0);
        }

        [Fact]
        public void Add_TrimsWhitespaceInValue()
        {
            var sourcedDictionary = new SourcedDictionary<string, string> { { "a", "b", " c " } };
            sourcedDictionary.Count.Should().Be(1);
            sourcedDictionary.ToList()[0].Value.Should().Be("c");
        }

        [Fact]
        public void Add_TrimsWhitespaceInValue_KeyValuePair()
        {
            var sourcedDictionary = new SourcedDictionary<string, string>();
            var data = new SourcedKeyValuePair<string, string>()
            {
                Key = "a",
                Source = "b",
                Value = " c "

            };
            sourcedDictionary.Add(data);
            sourcedDictionary.Count.Should().Be(1);
            sourcedDictionary.ToList()[0].Value.Should().Be("c");
        }

        [Fact]
        public void Add_OverwritesExistingValue_WhenSourceAndKeyMatch()
        {
            var sourcedDictionary = new SourcedDictionary<string, string>
            {
                { "a", "b", " c " },
                { "a", "b", " d " }
            };
            sourcedDictionary.Count.Should().Be(1);
            sourcedDictionary.ToList()[0].Value.Should().Be("d");
        }

        [Fact]
        public void Clear_RemovesAllKeyValuePairs()
        {
            var sourcedDictionary = new SourcedDictionary<string, string>
            {
                { "a", "b", " c " },
                { "a", "b", " d " }
            };
            sourcedDictionary.Count.Should().Be(1);

            sourcedDictionary.Clear();
            sourcedDictionary.Count.Should().Be(0);
        }

        [Fact]
        public void Remove_RemovesAllKeyValuePairs()
        {
            var sourcedDictionary = new SourcedDictionary<string, string>
            {
                {"a", "b", " c " },
                { "a", "c", " d " }
            };

            sourcedDictionary.Count.Should().Be(2);
            var data = new SourcedKeyValuePair<string, string>()
            {
                Source = "a",
                Key = "b",
                Value = "c"

            };
            sourcedDictionary.Remove(data);
            sourcedDictionary.Count.Should().Be(1);

        }

        [Fact]
        public void Contains_ReturnsTrue()
        {
            var sourcedDictionary = new SourcedDictionary<string, string>
            {
                {"a", "b", " c " },
                 };


            var data = new SourcedKeyValuePair<string, string>()
            {
                Source = "a",
                Key = "b",
                Value = "c"

            };
            sourcedDictionary.Contains(data).Should().BeTrue();
        }

        [Fact]
        public void Contains_ReturnsFalse()
        {
            var sourcedDictionary = new SourcedDictionary<string, string>
            {
                {"a", "b", " c " },
            };


            var data = new SourcedKeyValuePair<string, string>()
            {
                Source = "a",
                Key = "b",
                Value = "d"

            };
            sourcedDictionary.Contains(data).Should().BeFalse();
        }

        [Fact]
        public void GetEnumerator()
        {

            var sourcedDictionary = new SourcedDictionary<string, string>
            {
                {"a", "b", " c " },
            };

            foreach (var kvp in sourcedDictionary)
            {
                kvp.Source.Should().Be("a");
                kvp.Key.Should().Be("b");
                kvp.Value.Should().Be("c");

            }
        }
    }
}

