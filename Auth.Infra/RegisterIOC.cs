using Auth.Application;
using Galaxy.Application;
using ITGAcc.Integration.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infra {
    public static class RegisterIOC {
        /// <summary>
        /// Registers Auth infrastructure services and unit of work dependencies.
        /// </summary>
        /// <param name="services">The service collection to register into.</param>
        public static void RegisterInfra(this IServiceCollection services) {
            services.AddScoped<AuthUow>();
            services.AddScoped<IAuthUow>(provider => provider.GetRequiredService<AuthUow>());
            services.AddScoped<IUow>(provider => provider.GetRequiredService<AuthUow>());
            services.AddScoped<IBaseAccountsUow>(provider => provider.GetRequiredService<AuthUow>());
        }
    }
}
