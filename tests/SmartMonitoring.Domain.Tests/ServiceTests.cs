using SmartMonitoring.Domain.Exceptions;
using SmartMonitoring.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartMonitoring.Domain.Tests
{
    public class ServiceTests
    {
        [Fact]
        public void ShouldThrowForNullListOfLabels()
        {
            var name = new Name("service1");
            var port = new Port(8080);
            var maintainer = new Email("test@gmail.com");

            Assert.Throws<InvalidLabelException>(() => new Service(name, port, maintainer, null));
        }

        [Fact]
        public void ShouldThrowForEmptyListOfLabels()
        {
            var name = new Name("service1");
            var port = new Port(8080);
            var maintainer = new Email("test@gmail.com");
            var labels = new List<Label>();

            Assert.Throws<InvalidLabelException>(() => new Service(name, port, maintainer, labels));
        }

        [Fact]
        public void ShouldCreateWithMandatoryFields()
        {
            var name = new Name("service1");
            var port = new Port(8080);
            var maintainer = new Email("test@gmail.com");

            var labels = new List<Label>
            {
                new Label("key1:value1"),
                new Label("key2:value2")
            };

            var service = new Service(name, port, maintainer, labels);

            Assert.Equal("service1", service.Name.ToString());
            Assert.Equal("8080", service.Port.ToString());
            Assert.Equal("test@gmail.com", service.Maintainer.ToString());
            Assert.Equal(2, service.Labels.Count());
            Assert.Equal("key1:value1", service.Labels.ElementAt(0).ToString());
            Assert.Equal("key2:value2", service.Labels.ElementAt(1).ToString());
        }
    }
}
