using Common.HttpClient;
using Microsoft.Extensions.DependencyInjection;

namespace LCMS.S2SLogic
{
    public static class S2SLog
    {
        public static void RegisterS2SLogic(this IServiceCollection services)
        {
            services.RegisterHttpServiceClient();
            services.AddScoped<IS2SLogic, S2SLogic>();
            services.AddScoped<AdminS2SLogic>();
        }
    }
}
