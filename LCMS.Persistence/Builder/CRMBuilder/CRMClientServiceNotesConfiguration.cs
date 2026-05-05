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
    public class CRMClientServiceNotesConfiguration : BaseEntityConfiguration<CRMClientServiceNotes>
    {
        public override void Configure(EntityTypeBuilder<CRMClientServiceNotes> builder)
        {
            ConfigureModelProperties(builder);
            ConfigureModelRelationships(builder);
            ConfigureModelDescriptionProperties(builder);

            base.Configure(builder);
        }

        private void ConfigureModelDescriptionProperties(EntityTypeBuilder<CRMClientServiceNotes> builder)
        {
           
            builder.HasOne(p => p.EnteredByFullName)
               .WithMany()
               .HasPrincipalKey(u => new { u.UserLoginId })
               .HasForeignKey(p => new { p.EnteredBy })
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired(false);
        }

        private void ConfigureModelRelationships(EntityTypeBuilder<CRMClientServiceNotes> builder)
        {
            builder.HasOne(p => p.CRMClientService)
              .WithMany(c => c.CRMClientServiceNotes)
              .HasForeignKey(p => p.ClientServiceId)
              .OnDelete(DeleteBehavior.Cascade)
              .IsRequired(true);
        }

        private void ConfigureModelProperties(EntityTypeBuilder<CRMClientServiceNotes> builder)
        {
            builder.ToTable("CRMClientServiceNotes");
            builder.HasKey(p => p.Id);          
            builder.Property(p => p.EnteredOn).HasColumnType("datetime");
            builder.Property(p => p.EnteredBy).HasMaxLength(50);
        }
    }
}
