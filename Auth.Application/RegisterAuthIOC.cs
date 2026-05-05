using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application {
    public static class RegisterAuthIOC {
        public static void RegisterAuthServices(this IServiceCollection services) {
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
