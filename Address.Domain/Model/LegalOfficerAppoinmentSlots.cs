using Galaxy.Domain.Models;

namespace CRM.Domain
{
    public class LegalOfficerAppoinmentSlots : BaseEntity
    {
        public long Id { get; set; }
        public long LegalOfficerId { get; set; }
        public string Slotdate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? IsBooked { get; set; }
    }
}