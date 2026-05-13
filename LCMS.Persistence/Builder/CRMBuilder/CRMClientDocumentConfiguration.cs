using CRM.Domain;
using Galaxy.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCMS.Persistence
{
    public class CRMClientDocumentConfiguration : BaseEntityConfiguration<CRMClientDocument>
    {
        public override void Configure(EntityTypeBuilder<CRMClientDocument> builder)
        {
            ConfigureModelProperties(builder);
            ConfigureModelRelationships(builder);
            base.Configure(builder);
        }

        private void ConfigureModelProperties(EntityTypeBuilder<CRMClientDocument> builder)
        {
            builder.ToTable("CRMClientDocument");
            builder.HasKey(x => x.Id);

        }
        private void ConfigureModelRelationships(EntityTypeBuilder<CRMClientDocument> builder)
        {
            //builder.HasOne(p => p.Client)
            //.WithMany(c => c.CRMClientDocuments)
            //.HasForeignKey(c => c.ClientId)
            //.OnDelete(DeleteBehavior.Cascade)
            //.IsRequired(false);

        }
    }
}
