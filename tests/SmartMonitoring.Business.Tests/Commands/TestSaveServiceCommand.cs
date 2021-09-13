using SmartMonitoring.Business.Commands;
using SmartMonitoring.Domain.ValueObjects;
using System.Collections.Generic;

namespace SmartMonitoring.Business.Tests.Commands
{
    public class TestSaveServiceCommand : IServiceCommand
    {
        public Name Name { get; set; }
        public Port Port { get; set; }
        public Email Maintainer { get; set; }
        public IEnumerable<Label> Labels { get; set; }

        public static TestSaveServiceCommand Create(string name = "service1", int port = 8080)
        {
            return new TestSaveServiceCommand
            {
                Name = name,
                Port = port,
                Maintainer = "test@gmail.com",
                Labels = new List<Label>
                {
                    "key1:value1",
                    "key2:value2"
                }
            };
        }
    }
}
