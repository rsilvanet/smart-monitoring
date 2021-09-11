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
            Assert.Equal(value, new Name(value).ToString());
        }

        [Theory]
        [InlineData(" value")]
        [InlineData("value ")]
        [InlineData(" value ")]
        public void ShouldTrimTheValue(string value)
        {
            Assert.Equal(value.Trim(), new Name(value).ToString());
        }
    }
}
