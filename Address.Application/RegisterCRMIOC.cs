using Microsoft.Extensions.DependencyInjection;

namespace CRM.Application
{
    public static class RegisterCRMIOC
    {
        public static void RegisterMasterServices(this IServiceCollection services)
        {
            services.AddScoped<IConfigService, ConfigService>();
            services.AddScoped<IAddressService, AddressService>();           
            services.AddScoped<IDocumentFileService, DocumentFileService>();
            services.AddScoped<ICRMClientService, CRMClientServices>();
            
        }
    }
}