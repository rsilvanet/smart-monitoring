using System;
using System.Collections.Generic;

namespace SmartMonitoring.MemoryDatabase.Entities
{
    public class Service
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Port { get; set; }
        public string Maintainer { get; set; }
        public IEnumerable<Label> Labels { get; set; }
    }
}
