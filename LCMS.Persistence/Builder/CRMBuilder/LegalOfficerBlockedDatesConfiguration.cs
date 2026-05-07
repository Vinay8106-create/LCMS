using CRM.Domain;
using Galaxy.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCMS.Persistence
{
    public class LegalOfficerBlockedDatesConfiguration : BaseEntityConfiguration<LegalOfficerBlockedDates>
    {
        public override void Configure(EntityTypeBuilder<LegalOfficerBlockedDates> builder)
        {
            ConfigureModelProperties(builder);
            base.Configure(builder);
        }

        public virtual void ConfigureModelProperties(EntityTypeBuilder<LegalOfficerBlockedDates> builder)
        {
            builder.ToTable("LegalOfficerBlockedDates");

        }

    }
}
