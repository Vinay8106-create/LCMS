using Galaxy.Domain.Models;

namespace CRM.Domain
{
    public class CRMClientDocument : BaseEntity
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public long? DocumentMasterId { get; set; }
        public long? DocumentId { get; set; }
        //public virtual Collection<Document>? Documents { get; set; }

    }
}
