using SmartMonitoring.Business.Commands;
using SmartMonitoring.Business.Exceptions;
using SmartMonitoring.Business.Repositories;
using SmartMonitoring.Domain;
using SmartMonitoring.Domain.ValueObjects;
using System.Threading.Tasks;

namespace SmartMonitoring.Business.UseCases.Create
{
    public class UpdateServiceUseCase
    {
        private readonly IServiceRepository _serviceRepository;

        public UpdateServiceUseCase(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Service> ExecuteAsync(Name name, ISaveServiceCommand command)
        {
            var service = await _serviceRepository.GetByNameAsync(name);

            if (service == null)
            {
                throw new ServiceNotFoundException(name);
            }

            await ValidateNameChangeAsync(name, command.Name);

            service.Update(command.Name, command.Port, command.Maintainer, command.Labels);

            await _serviceRepository.UpdateAsync(service);

            return service;
        }

        private async Task ValidateNameChangeAsync(Name currentName, Name newName)
        {
            var nameWasChanged = !currentName.Equals(newName);

            if (nameWasChanged)
            {
                var newNameAlreadyInUse = await _serviceRepository.ExistsAsync(newName);

                if (newNameAlreadyInUse)
                {
                    throw new ServiceNameAlreadyInUseException(newName);
                }
            }
        }
    }
}
