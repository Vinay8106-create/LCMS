using CRM.Domain;
using Microsoft.EntityFrameworkCore;

namespace LCMS.Persistence
{
    public static class CRMModelRegistrar
    {
        public static void RegisterEntityForModelCreation(ModelBuilder modelBuilder)
        {
            RegisterAddressEntityForModelCreation(modelBuilder);
            RegisterViewsForSearch(modelBuilder);
        }

        private static void RegisterAddressEntityForModelCreation(ModelBuilder modelBuilder)
        {
            new ConfigConfiguration().Configure(modelBuilder.Entity<config_ClientType>());
            new AddressConfiguration().Configure(modelBuilder.Entity<Address>());
            new CRMClientConfiguration().Configure(modelBuilder.Entity<CRMClient>());
            new CRMClientContactConfiguration().Configure(modelBuilder.Entity<CRMClientContact>());
            new CRMClientDocumentConfiguration().Configure(modelBuilder.Entity<CRMClientDocument>());

            new LegalOfficerConfiguration().Configure(modelBuilder.Entity<LegalOfficer>());
        }

        private static void RegisterViewsForSearch(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CRMClientSearchViewModel>(entity => {
                entity.HasNoKey();
                entity.ToView("vw_ClientSearch");
            });

            modelBuilder.Entity<CRMClientServiceSearchViewModel>(entity => {
                entity.HasNoKey();
                entity.ToView("vw_ClientServiceSearch");
            });

            modelBuilder.Entity<LegalOfficerSearchViewModel>(entity => {
                entity.HasNoKey();
                entity.ToView("vw_LegalOfficerSearch");
            });

            modelBuilder.Entity<BlockedDateSearchViewModel>(entity => {
                entity.HasNoKey();
                entity.ToView("vw_BlockedDateSearch");
            });

        }
    }
}
