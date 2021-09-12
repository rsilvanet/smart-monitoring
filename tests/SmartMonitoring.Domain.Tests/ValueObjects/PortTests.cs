using SmartMonitoring.Domain.Exceptions;
using SmartMonitoring.Domain.ValueObjects;
using Xunit;

namespace SmartMonitoring.Domain.Tests
{
    public class PortTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(65536)]
        public void ShouldThrowForNumberOutOfBoundaries(int value)
        {
            Assert.Throws<InvalidPortException>(() => new Port(value));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(65535)]
        public void ShouldCreateForValidNumber(int value)
        {
            Assert.Equal<int>(value, new Port(value));
        }
    }
}
