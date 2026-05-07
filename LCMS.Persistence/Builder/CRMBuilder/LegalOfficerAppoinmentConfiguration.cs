using CRM.Domain;
using Galaxy.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCMS.Persistence
{
    public class LegalOfficerAppoinmentConfiguration : BaseEntityConfiguration<LegalOfficerAppoinment>
    {
        public override void Configure(EntityTypeBuilder<LegalOfficerAppoinment> builder)
        {
            ConfigureModelProperties(builder);
            base.Configure(builder);
        }

        public virtual void ConfigureModelProperties(EntityTypeBuilder<LegalOfficerAppoinment> builder)
        {
            builder.ToTable("LegalOfficerAppoinment");

        }

    }
}
