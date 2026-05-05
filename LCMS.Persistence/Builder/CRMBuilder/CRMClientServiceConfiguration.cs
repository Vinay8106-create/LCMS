using CRM.Domain;
using Galaxy.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCMS.Persistence
{
    public class CRMClientServiceConfiguration : BaseEntityConfiguration<CRMClientService>
    {
        public override void Configure(EntityTypeBuilder<CRMClientService> builder)
        {
            ConfigureModelProperties(builder);
            ConfigureModelRelationships(builder);
            base.Configure(builder);
        }

        public virtual void ConfigureModelProperties(EntityTypeBuilder<CRMClientService> builder)
        {
            builder.ToTable("CRMClientService");
            builder.Property(c => c.ServiceRefNo).HasMaxLength(20);          
        }
        private void ConfigureModelRelationships(EntityTypeBuilder<CRMClientService> builder)
        {
           
            builder.HasMany(p => p.CRMClientServiceStatusHistories)
               .WithOne(c => c.CRMClientService)
               .HasPrincipalKey(p => p.Id)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired(false);
            builder.HasMany(p => p.CRMClientServiceEmailHistories)
               .WithOne(c => c.CRMClientService)
               .HasPrincipalKey(p => p.Id)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired(false);
            builder.HasMany(p => p.CRMClientServiceAssignedOfficers)
              .WithOne(c => c.CRMClientService)
              .HasPrincipalKey(p => p.Id)
              .OnDelete(DeleteBehavior.Cascade)
              .IsRequired(false);
        }
    }
}
