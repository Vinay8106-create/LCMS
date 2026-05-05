using CRM.Domain;
using Galaxy.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCMS.Persistence
{
    public class ConfigConfiguration : BaseEntityConfiguration<config_ClientType>
    {
        public override void Configure(EntityTypeBuilder<config_ClientType> builder)
        {

            base.Configure(builder);
        }

        public virtual void ConfigureModelProperties(EntityTypeBuilder<config_ClientType> builder)
        {
            builder.ToTable("config_ClientType");

            builder.Property(a => a.Description).HasMaxLength(500);
        }
    }
}

