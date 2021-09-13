using SmartMonitoring.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SmartMonitoring.API.Models.Responses
{
    public class ServiceResponse
    {
        public ServiceResponse(Service service)
        {
            Name = service.Name;
            Port = service.Port;
            Maintainer = service.Maintainer;
            Labels = service.Labels.Select(l => l.ToString());
        }

        public string Name { get; }
        public int Port { get; }
        public string Maintainer { get; }
        public IEnumerable<string> Labels { get; }
    }
}
