using SmartMonitoring.Domain.ValueObjects;
using System;

namespace SmartMonitoring.Business.Exceptions
{
    public class ServiceNameAlreadyInUseException : Exception
    {
        public ServiceNameAlreadyInUseException(Name name) : base($"There's already a service registered with the name {name}.") { }
    }
}
