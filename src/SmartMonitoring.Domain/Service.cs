using SmartMonitoring.Domain.Exceptions;
using SmartMonitoring.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace SmartMonitoring.Domain
{
    public class Service
    {
        public Service(Name name, Port port, Email maintainer, IEnumerable<Label> labels)
        {
            if (!HasAny(labels))
            {
                throw new InvalidLabelException();
            }

            Name = name;
            Port = port;
            Maintainer = maintainer;
            Labels = labels;
        }

        public Name Name { get; }
        public Port Port { get; }
        public Email Maintainer { get; }
        public IEnumerable<Label> Labels { get; }

        private bool HasAny(IEnumerable<Label> labels)
        {
            return labels?.Any() == true;
        }
    }
}
