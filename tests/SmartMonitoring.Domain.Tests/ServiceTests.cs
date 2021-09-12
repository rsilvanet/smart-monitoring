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

            Assert.Throws<EmptyLabelListException>(() => new Service(name, port, maintainer, null));
        }

        [Fact]
        public void ShouldThrowForEmptyListOfLabels()
        {
            var name = new Name("service1");
            var port = new Port(8080);
            var maintainer = new Email("test@gmail.com");
            var labels = new List<Label>();

            Assert.Throws<EmptyLabelListException>(() => new Service(name, port, maintainer, labels));
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

            Assert.Equal("service1", service.Name);
            Assert.Equal<int>(8080, service.Port);
            Assert.Equal("test@gmail.com", service.Maintainer);
            Assert.Equal(2, service.Labels.Count());
            Assert.Equal("key1:value1", service.Labels.ElementAt(0));
            Assert.Equal("key2:value2", service.Labels.ElementAt(1));
        }

        [Fact]
        public void ShouldCreateUsingImplicitOperations()
        {
            var labels = new List<Label>
            {
                "key1:value1",
                "key2:value2"
            };

            var service = new Service("service1", 8080, "test@gmail.com", labels);

            Assert.Equal("service1", service.Name);
            Assert.Equal<int>(8080, service.Port);
            Assert.Equal("test@gmail.com", service.Maintainer);
            Assert.Equal(2, service.Labels.Count());
            Assert.Equal("key1:value1", service.Labels.ElementAt(0));
            Assert.Equal("key2:value2", service.Labels.ElementAt(1));
        }

        [Fact]
        public void ShouldUpdateMandatoryFields()
        {
            var labels = new List<Label>
            {
                "key1:value1",
                "key2:value2"
            };

            var service = new Service("service1", 8080, "test@gmail.com", labels);

            Assert.Equal("service1", service.Name);
            Assert.Equal<int>(8080, service.Port);
            Assert.Equal("test@gmail.com", service.Maintainer);
            Assert.Equal(2, service.Labels.Count());

            var newLabels = new List<Label>
            {
                "key3:value3"
            };

            service.Update("service2", 8081, "test2@gmail.com", newLabels);

            Assert.Equal("service2", service.Name);
            Assert.Equal<int>(8081, service.Port);
            Assert.Equal("test2@gmail.com", service.Maintainer);
            Assert.Single(service.Labels);
            Assert.Equal("key3:value3", service.Labels.ElementAt(0));
        }
    }
}
