using CRM.Domain;
using Galaxy.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCMS.Persistence
{
    public class LegalOfficerConfiguration : BaseEntityConfiguration<LegalOfficer>
    {
        public override void Configure(EntityTypeBuilder<LegalOfficer> builder)
        {
            ConfigureModelProperties(builder);
            ConfigureModelRelationships(builder);
            base.Configure(builder);
        }

        public virtual void ConfigureModelProperties(EntityTypeBuilder<LegalOfficer> builder)
        {
            builder.ToTable("LegalOfficer");
            builder.Property(c => c.RegNumber).HasMaxLength(20);
        }
        private void ConfigureModelRelationships(EntityTypeBuilder<LegalOfficer> builder)
        {

            //builder
            //.HasOne(c => c.Photo)
            //.WithMany() // assuming Document is reusable
            //.HasForeignKey(c => c.PhotoId)
            //.HasPrincipalKey(d => d.Id);


            // Photo (nullable FK - long?)
            builder
                .HasOne(c => c.Photo)
                .WithMany()
                .HasForeignKey(c => c.PhotoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Doc (non-nullable FK - long)
            builder
                .HasOne(c => c.Doc)
                .WithMany()
                .HasForeignKey(c => c.IDDocId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
