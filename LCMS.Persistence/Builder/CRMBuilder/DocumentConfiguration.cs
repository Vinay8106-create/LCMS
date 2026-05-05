using Galaxy.Infra.EntityConfig;
using CRM.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCMS.Persistence
{
    public class DocumentConfiguration : BaseEntityConfiguration<Document>
    {
        public override void Configure(EntityTypeBuilder<Document> builder)
        {

            base.Configure(builder);
        }

        public virtual void ConfigureModelProperties(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Document");

            builder.Property(a => a.FileName).HasMaxLength(200);

           
        }


    }
}

