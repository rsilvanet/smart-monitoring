using SmartMonitoring.Business.Commands;
using SmartMonitoring.Domain.ValueObjects;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmartMonitoring.API.Models.Requests
{
    public class ServiceRequest : IServiceCommand
    {
        [Required]
        [MinLength(Domain.ValueObjects.Name.MIN_LENGTH)]
        [MaxLength(Domain.ValueObjects.Name.MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [Range(Domain.ValueObjects.Port.MIN_PORT_NUMBER, Domain.ValueObjects.Port.MAX_PORT_NUMBER)]
        public int Port { get; set; }

        [Required]
        [EmailAddress]
        public string Maintainer { get; set; }

        [Required]
        [MinLength(1)]
        public IEnumerable<string> Labels { get; set; }

        Name IServiceCommand.Name => Name;
        Port IServiceCommand.Port => Port;
        Email IServiceCommand.Maintainer => Maintainer;
        IEnumerable<Label> IServiceCommand.Labels => Labels.Select(l => new Label(l));
    }
}
