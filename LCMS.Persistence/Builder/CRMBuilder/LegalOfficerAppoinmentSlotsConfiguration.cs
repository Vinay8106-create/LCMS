using CRM.Domain;
using Galaxy.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCMS.Persistence
{
    public class LegalOfficerAppoinmentSlotsConfiguration : BaseEntityConfiguration<LegalOfficerAppoinmentSlots>
    {
        public override void Configure(EntityTypeBuilder<LegalOfficerAppoinmentSlots> builder)
        {
            ConfigureModelProperties(builder);
            base.Configure(builder);
        }

        public virtual void ConfigureModelProperties(EntityTypeBuilder<LegalOfficerAppoinmentSlots> builder)
        {
            builder.ToTable("LegalOfficerAppoinmentSlots");

        }

    }
}
