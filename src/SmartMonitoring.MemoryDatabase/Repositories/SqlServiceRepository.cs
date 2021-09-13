using Microsoft.EntityFrameworkCore;
using SmartMonitoring.Business.Repositories;
using SmartMonitoring.Domain;
using SmartMonitoring.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMonitoring.MemoryDatabase.Repositories
{
    public class SqlServiceRepository : IServiceRepository
    {
        private readonly SmartMonitoringDbContext _dbContext;

        public SqlServiceRepository(SmartMonitoringDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private Service ToDomain(Entities.Service service)
        {
            return Service.Load(
                service.Id,
                service.Name,
                service.Port,
                service.Maintainer,
                service.Labels.Select(x => new Label(x.Value))
            );
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            var dbServices = await _dbContext.Services
                .Include(s => s.Labels)
                .ToListAsync();

            return dbServices.Select(s => ToDomain(s));
        }

        public async Task<IEnumerable<Service>> GetByLabelAsync(Label label)
        {
            var dbServices = await _dbContext.Services
                .Include(s => s.Labels)
                .Where(s => s.Labels.Any(l => l.Value == label))
                .ToListAsync();

            return dbServices.Select(s => ToDomain(s));
        }

        public async Task<bool> ExistsAsync(Name name)
        {
            return await _dbContext.Services.AnyAsync(s => s.Name == name);
        }

        public async Task<Service> GetByNameAsync(Name name)
        {
            var dbService = await _dbContext.Services
                .Include(s => s.Labels)
                .Where(s => s.Name == name)
                .SingleOrDefaultAsync();

            if (dbService != null)
            {
                return ToDomain(dbService);
            }

            return null;
        }

        public async Task AddAsync(Service service)
        {
            var entity = new Entities.Service
            {
                Id = service.Id,
                Name = service.Name,
                Port = service.Port,
                Maintainer = service.Maintainer,
                Labels = service.Labels.Select(label => new Entities.Label(label)).ToList()
            };

            await _dbContext.Services.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Service service)
        {
            var entity = await _dbContext.Services
                .Include(s => s.Labels)
                .Where(s => s.Id == service.Id)
                .SingleAsync();

            entity.Name = service.Name;
            entity.Port = service.Port;
            entity.Maintainer = service.Maintainer;
            entity.Labels = service.Labels.Select(label => new Entities.Label(label)).ToList();

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Name name)
        {
            var entity = await _dbContext.Services.SingleAsync(x => x.Name == name);

            _dbContext.Services.Remove(entity);

            await _dbContext.SaveChangesAsync();
        }
    }
}
