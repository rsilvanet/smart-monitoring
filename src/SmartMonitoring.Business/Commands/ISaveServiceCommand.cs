using SmartMonitoring.Domain.ValueObjects;
using System.Collections.Generic;

namespace SmartMonitoring.Business.Commands
{
    public interface ISaveServiceCommand
    {
        Name Name { get; set; }
        Port Port { get; set; }
        Email Maintainer { get; set; }
        IEnumerable<Label> Labels { get; set; }
    }
}
