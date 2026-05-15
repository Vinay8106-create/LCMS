using CRM.Domain;
using Galaxy.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCMS.Persistence
{
    public class CRMClientConfiguration : BaseEntityConfiguration<CRMClient>
    {
        public override void Configure(EntityTypeBuilder<CRMClient> builder)
        {
            ConfigureModelProperties(builder);
            ConfigureModelRelationships(builder);
            base.Configure(builder);
        }

        public virtual void ConfigureModelProperties(EntityTypeBuilder<CRMClient> builder)
        {
            builder.ToTable("CRMClient");
            builder.Property(c => c.FirstName).HasMaxLength(250);
            builder.Property(c => c.LastName).HasMaxLength(250);
            builder.Property(c => c.ContactNo).HasMaxLength(20);
            builder.Property(c => c.EmailId).HasMaxLength(250);
        }
        private void ConfigureModelRelationships(EntityTypeBuilder<CRMClient> builder)
        {
            //builder.HasMany(p => p.CRMClientContacts)
            //.WithOne(c => c.Client)
            //.HasForeignKey(c => c.ClientId)
            //.OnDelete(DeleteBehavior.Cascade)
            //.IsRequired(false);

            // builder.HasMany(p => p.CRMClientDocuments)
            //.WithOne(c => c.Client)
            //.HasForeignKey(c => c.ClientId)
            //.OnDelete(DeleteBehavior.Cascade)
            //.IsRequired(false);

            builder
            .HasOne(c => c.Photo)
            .WithMany() // assuming Document is reusable
            .HasForeignKey(c => c.PhotoId)
            .HasForeignKey(d => d.Id);
        }
    }
}
