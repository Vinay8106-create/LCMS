using Galaxy.Application;
using CRM.Application;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Infra
{
    public static class IOC
    {
        public static void RegisterInfra(this IServiceCollection service)
        {
            service.AddScoped<ICRMUow, CRMUow>();
            service.AddScoped<IUow>(x => x.GetRequiredService<ICRMUow>());
        }
    }
}