using SmartMonitoring.Domain.Exceptions;
using SmartMonitoring.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartMonitoring.Domain
{
    public class Service
    {
        private Service(Guid id)
        {
            Id = id;
        }

        public Service(Name name, Port port, Email maintainer, IEnumerable<Label> labels)
        {
            Id = Guid.NewGuid();
            Update(name, port, maintainer, labels);
        }

        public Guid Id { get; private set; }
        public Name Name { get; private set; }
        public Port Port { get; private set; }
        public Email Maintainer { get; private set; }
        public IEnumerable<Label> Labels { get; private set; }

        private bool HasAny(IEnumerable<Label> labels)
        {
            return labels?.Any() == true;
        }

        public void Update(Name name, Port port, Email maintainer, IEnumerable<Label> labels)
        {
            if (!HasAny(labels))
            {
                throw new EmptyLabelListException();
            }

            Name = name;
            Port = port;
            Maintainer = maintainer;
            Labels = labels;
        }

        public static Service Load(Guid id, Name name, Port port, Email maintainer, IEnumerable<Label> labels)
        {
            var service = new Service(id);
            service.Update(name, port, maintainer, labels);
            return service;
        }
    }
}
