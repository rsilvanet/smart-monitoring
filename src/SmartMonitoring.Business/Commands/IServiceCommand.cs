using SmartMonitoring.Domain.ValueObjects;
using System.Collections.Generic;

namespace SmartMonitoring.Business.Commands
{
    public interface IServiceCommand
    {
        Name Name { get; }
        Port Port { get; }
        Email Maintainer { get; }
        IEnumerable<Label> Labels { get; }
    }
}
