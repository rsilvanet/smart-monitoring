using SmartMonitoring.Business.Exceptions;
using SmartMonitoring.Business.Repositories;
using SmartMonitoring.Domain.ValueObjects;
using System.Threading.Tasks;

namespace SmartMonitoring.Business.UseCases.Create
{
    public class DeleteServiceUseCase
    {
        private readonly IServiceRepository _serviceRepository;

        public DeleteServiceUseCase(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task ExecuteAsync(Name name)
        {
            var serviceExists = await _serviceRepository.ExistsAsync(name);

            if (!serviceExists)
            {
                throw new ServiceNotFoundException(name);
            }

            await _serviceRepository.DeleteAsync(name);
        }
    }
}
