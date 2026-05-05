using CRM.Domain;
using Galaxy.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCMS.Persistence
{
    public class CRMClientContactConfiguration : BaseEntityConfiguration<CRMClientContact>
    {
        public override void Configure(EntityTypeBuilder<CRMClientContact> builder)
        {
            ConfigureModelProperties(builder);
            ConfigureModelRelationships(builder);

            base.Configure(builder);
        }

        private void ConfigureModelProperties(EntityTypeBuilder<CRMClientContact> builder)
        {
            builder.ToTable("CRMClientContact");
            builder.HasKey(x => x.Id);
        }

        private void ConfigureModelRelationships(EntityTypeBuilder<CRMClientContact> builder)
        {
            //builder.HasOne(p => p.Client)
            //.WithMany(c => c.CRMClientContacts)
            //.HasForeignKey(c => c.ClientId)
            //.OnDelete(DeleteBehavior.Cascade)
            //.IsRequired(false);

        }
    }
}
