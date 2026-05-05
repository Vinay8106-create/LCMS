using Galaxy.Infra.EntityConfig;
using CRM.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCMS.Persistence
{
    public class AddressConfiguration : BaseEntityConfiguration<Address>
    {
        public override void Configure(EntityTypeBuilder<Address> builder)
        {
            
            base.Configure(builder);
        }

        public virtual void ConfigureModelProperties(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");

            builder.Property(a => a.Line1).HasMaxLength(200);

            builder.Property(a => a.Line2).HasMaxLength(200);

            builder.Property(a => a.Line3).HasMaxLength(200);
           

        }

       
    }
}

