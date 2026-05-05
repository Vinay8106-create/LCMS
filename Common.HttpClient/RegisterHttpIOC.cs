using Microsoft.Extensions.DependencyInjection;

namespace Common.HttpClient
{
    public static class RegisterHttpIOC
    {
        public static void RegisterHttpServiceClient(this IServiceCollection services)
        {
            services.AddScoped<IHttpServiceClient, HttpServiceClient>();
            services.AddHttpClient();
        }
    }
}