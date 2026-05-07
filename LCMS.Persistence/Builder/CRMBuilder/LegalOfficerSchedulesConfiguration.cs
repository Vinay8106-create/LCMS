using CRM.Domain;
using Galaxy.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCMS.Persistence
{
    public class LegalOfficerSchedulesConfiguration : BaseEntityConfiguration<LegalOfficerSchedules>
    {
        public override void Configure(EntityTypeBuilder<LegalOfficerSchedules> builder)
        {
            ConfigureModelProperties(builder);           
            base.Configure(builder);
        }

        public virtual void ConfigureModelProperties(EntityTypeBuilder<LegalOfficerSchedules> builder)
        {
            builder.ToTable("LegalOfficerSchedules");
           
        }
       
    }
}
