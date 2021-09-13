using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartMonitoring.Business.Repositories;
using SmartMonitoring.MemoryDatabase.Repositories;

namespace SmartMonitoring.MemoryDatabase
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMemorydDatabase(this IServiceCollection services)
        {
            services.AddDbContext<SmartMonitoringDbContext>(options =>
            {
                options.UseInMemoryDatabase("SmartMonitoring");
            });

            services.AddScoped<IServiceRepository, SqlServiceRepository>();
        }
    }
}
