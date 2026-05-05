using Galaxy.Infra.EntityConfig;
using LCMS.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galaxy.Domain.Models;

namespace LCMS.Persistence
{
    public class CRMClientServiceEmailHistoryConfiguration : BaseEntityConfiguration<CRMClientServiceEmailHistory>
    {
        public override void Configure(EntityTypeBuilder<CRMClientServiceEmailHistory> builder)
        {
            ConfigureModelProperties(builder);
            ConfigureModelDescriptionProperties(builder);
            ConfigureModelRelationships(builder);
            base.Configure(builder);
        }

        public static void ConfigureModelProperties(EntityTypeBuilder<CRMClientServiceEmailHistory> builder)
        {
            builder.ToTable("CRMClientServiceEmailHistory");
            builder.HasKey(p => p.Id);
          
        }
        public static void ConfigureModelDescriptionProperties(EntityTypeBuilder<CRMClientServiceEmailHistory> builder)
        {
           
        }
        public static void ConfigureModelRelationships(EntityTypeBuilder<CRMClientServiceEmailHistory> builder)
        {
            builder.HasOne(p => p.CRMClientService)
            .WithMany(c => c.CRMClientServiceEmailHistories)
            .HasForeignKey(p => p.ClientServiceId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);

            builder.HasOne(re => re.CommunicationTracking).WithOne()
            .HasPrincipalKey<CommunicationTracking>(re => new { re.Id })
            .HasForeignKey<CRMClientServiceEmailHistory>(x => new { x.EmailTrackingId })
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        }
    }
}
