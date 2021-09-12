using SmartMonitoring.Business.Commands;
using SmartMonitoring.Business.Exceptions;
using SmartMonitoring.Business.Repositories;
using SmartMonitoring.Domain;
using System.Threading.Tasks;

namespace SmartMonitoring.Business.UseCases.Create
{
    public class CreateServiceUseCase
    {
        private readonly IServiceRepository _serviceRepository;

        public CreateServiceUseCase(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Service> ExecuteAsync(ISaveServiceCommand command)
        {
            var nameAlreadyInUse = await _serviceRepository.ExistsAsync(command.Name);

            if (nameAlreadyInUse)
            {
                throw new ServiceNameAlreadyInUseException(command.Name);
            }

            var service = new Service(command.Name, command.Port, command.Maintainer, command.Labels);

            await _serviceRepository.AddAsync(service);

            return service;
        }
    }
}
