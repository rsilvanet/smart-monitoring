using SmartMonitoring.Domain.Exceptions;
using SmartMonitoring.Domain.ValueObjects;
using Xunit;

namespace SmartMonitoring.Domain.Tests
{
    public class EmailTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("test.gmail.com")]
        [InlineData("@gmail.com")]
        public void ShouldThrowForInvalidValue(string value)
        {
            Assert.Throws<InvalidEmailException>(() => new Email(value));
        }

        [Theory]
        [InlineData("test@kinly.nl")]
        [InlineData("test@gmail.com")]
        public void ShouldCreateForValidValue(string value)
        {
            Assert.Equal(value, new Email(value).ToString());
        }

        [Theory]
        [InlineData(" test@kinly.nl")]
        [InlineData("test@gmail.com ")]
        public void ShouldTrimTheValue(string value)
        {
            Assert.Equal(value.Trim(), new Email(value).ToString());
        }
    }
}
