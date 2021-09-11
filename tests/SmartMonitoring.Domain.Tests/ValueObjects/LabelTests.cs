using SmartMonitoring.Domain.Exceptions;
using SmartMonitoring.Domain.ValueObjects;
using Xunit;

namespace SmartMonitoring.Domain.Tests
{
    public class LabelTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("aaa")]
        [InlineData("a.b")]
        [InlineData("a:b:c")]
        public void ShouldThrowForInvalidValue(string value)
        {
            Assert.Throws<InvalidLabelException>(() => new Label(value));
        }

        [Theory]
        [InlineData("a:b")]
        [InlineData("key:value")]
        public void ShouldCreateForValidValue(string value)
        {
            Assert.Equal(value, new Label(value).ToString());
        }

        [Theory]
        [InlineData(" a:b")]
        [InlineData("key:value ")]
        public void ShouldTrimTheEmailValue(string value)
        {
            Assert.Equal(value.Trim(), new Label(value).ToString());
        }
    }
}
