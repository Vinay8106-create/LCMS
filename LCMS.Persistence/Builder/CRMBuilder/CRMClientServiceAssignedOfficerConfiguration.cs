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
    public class CRMClientServiceAssignedOfficerConfiguration : BaseEntityConfiguration<CRMClientServiceAssignedOfficer>
    {
        public override void Configure(EntityTypeBuilder<CRMClientServiceAssignedOfficer> builder)
        {
            ConfigureModelProperties(builder);
            ConfigureModelRelationships(builder);
            ConfigureModelDescriptionProperties(builder);

            base.Configure(builder);
        }

        private void ConfigureModelDescriptionProperties(EntityTypeBuilder<CRMClientServiceAssignedOfficer> builder)
        {
             builder.HasOne(p => p.AssignedByFullName)
            .WithMany()
            .HasPrincipalKey(u => new { u.UserLoginId })
            .HasForeignKey(p => new { p.AssignedBy })
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);

            builder.HasOne(p => p.AssignedToFullName)
            .WithMany()
            .HasPrincipalKey(u => new { u.UserLoginId })
            .HasForeignKey(p => new { p.AssignedTo })
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);
        }

        private void ConfigureModelRelationships(EntityTypeBuilder<CRMClientServiceAssignedOfficer> builder)
        {
            builder.HasOne(p => p.CRMClientService)
            .WithMany(c => c.CRMClientServiceAssignedOfficers)
            .HasForeignKey(p => p.ClientServiceId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);
        }

        private void ConfigureModelProperties(EntityTypeBuilder<CRMClientServiceAssignedOfficer> builder)
        {
            builder.ToTable("CRMClientServiceAssignedOfficer");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.AssignedBy).HasMaxLength(50);
            builder.Property(p => p.AssignedTo).HasMaxLength(50);
            builder.Property(p => p.AssignedDate).HasColumnType("datetime");
            
        }
    }
}
