using SmartMonitoring.Domain;
using SmartMonitoring.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartMonitoring.Business.Repositories
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>> GetAllAsync();
        Task<IEnumerable<Service>> GetByLabelAsync(Label label);
        Task<bool> ExistsAsync(Name name);
        Task<Service> GetByNameAsync(Name name);
        Task AddAsync(Service service);
        Task UpdateAsync(Service service);
        Task DeleteAsync(Name name);
    }
}
