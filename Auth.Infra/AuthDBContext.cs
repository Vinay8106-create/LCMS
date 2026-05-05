using Galaxy.Infra;
using ITGAcc.Integration.Infra;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infra {
    public class AuthDBContext : ITGDbContext {
        public AuthDBContext(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) {
        }

        /// <summary>
        /// Registers entities and configurations for the Auth database context.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure EF entities.</param>
        public override void RegisterEntityForModelCreation(ModelBuilder modelBuilder) {
            // Register models from ITG Accounts Integration
            modelBuilder.RegisterAccIntegrationModelBuilders();

            // Call base for additional shared registrations
            base.RegisterEntityForModelCreation(modelBuilder);
        }
    }
}
