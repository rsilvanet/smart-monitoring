using SmartMonitoring.Domain.ValueObjects;
using System.Collections.Generic;

namespace SmartMonitoring.Domain
{
    public class Service
    {
        public Service(Name name, Port port, Email maintainer, IEnumerable<Label> labels)
        {
            Name = name;
            Port = port;
            Maintainer = maintainer;
            Labels = labels ?? new List<Label>();
        }

        public Name Name { get; }
        public Port Port { get; }
        public Email Maintainer { get; }
        public IEnumerable<Label> Labels { get; }
    }
}
