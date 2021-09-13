using Microsoft.Extensions.DependencyInjection;
using SmartMonitoring.Business.UseCases.Create;

namespace SmartMonitoring.Business
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBusiness(this IServiceCollection services)
        {
            services.AddScoped<CreateServiceUseCase>();
            services.AddScoped<UpdateServiceUseCase>();
            services.AddScoped<DeleteServiceUseCase>();
        }
    }
}
