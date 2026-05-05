using Galaxy.Infra.EntityConfig;
using LCMS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCMS.Persistence
{
    public class CRMClientServiceStatusHistoryConfiguration : BaseEntityConfiguration<CRMClientServiceStatusHistory>
    {
        public override void Configure(EntityTypeBuilder<CRMClientServiceStatusHistory> builder)
        {
            ConfigureModelProperties(builder);
            ConfigureModelRelationships(builder);
            ConfigureModelDescriptionProperties(builder);

            base.Configure(builder);
        }

        private void ConfigureModelDescriptionProperties(EntityTypeBuilder<CRMClientServiceStatusHistory> builder)
        {
           
            builder.HasOne(p => p.ChangedByFullName)
                .WithMany()
                .HasPrincipalKey(u => new { u.UserLoginId })
                .HasForeignKey(p => new { p.ChangedBy })
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
        }

        private void ConfigureModelRelationships(EntityTypeBuilder<CRMClientServiceStatusHistory> builder)
        {
            builder.HasOne(p => p.CRMClientService)
            .WithMany(c => c.CRMClientServiceStatusHistories)
            .HasForeignKey(p => p.ClientServiceId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);
        }

        private void ConfigureModelProperties(EntityTypeBuilder<CRMClientServiceStatusHistory> builder)
        {
            builder.ToTable("CRMClientServiceStatusHistory");
            builder.HasKey(p => p.Id);            
            builder.Property(p => p.ChangedOn).HasColumnType("datetime");
            builder.Property(p => p.ChangedBy).HasMaxLength(50);
        }
    }
}
