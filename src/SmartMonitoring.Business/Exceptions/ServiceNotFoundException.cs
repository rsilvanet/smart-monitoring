using System;

namespace SmartMonitoring.Business.Exceptions
{
    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(string name) : base($"Service not found with name '{name}'.") { }
    }
}