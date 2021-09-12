using SmartMonitoring.Domain.Exceptions;
using SmartMonitoring.Domain.ValueObjects;
using Xunit;

namespace SmartMonitoring.Domain.Tests
{
    public class NameTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ShouldThrowForNullOrEmptyValue(string value)
        {
            Assert.Throws<InvalidNameException>(() => new Name(value));
        }

        [Theory]
        [InlineData("03c")]
        [InlineData("31charactersloooooooooooooooong")]
        public void ShouldThrowForLengthOutOfBoundaries(string value)
        {
            Assert.Throws<InvalidNameException>(() => new Name(value));
        }

        [Theory]
        [InlineData("04ch")]
        [InlineData("30characterslooooooooooooooong")]
        public void ShouldCreateForValidValue(string value)
        {
            Assert.Equal(value, new Name(value));
        }

        [Theory]
        [InlineData(" value")]
        [InlineData("value ")]
        [InlineData(" value ")]
        public void ShouldTrimTheValue(string value)
        {
            Assert.Equal(value.Trim(), new Name(value));
        }

        [Theory]
        [InlineData("name1", "name1", true)]
        [InlineData("name1", "name2", false)]
        public void ShouldBeAbleToCompareTwoValues(string value1, string value2, bool expected)
        {
            var name1 = new Name(value1);
            var name2 = new Name(value2);

            Assert.Equal(expected, name1 == name2);
            Assert.Equal(expected, name1 == value2);
            Assert.Equal(expected, name1.Equals(name2));
            Assert.Equal(expected, name1.Equals(value2));
            Assert.Equal(expected, value1.Equals(name2));
        }
    }
}
