using Galaxy.Domain.Models;

namespace CRM.Domain
{
    public class LegalOfficerAppoinment : BaseEntity
    {
        public long Id { get; set; }
        public string? AppointmentNo { get; set; }
        public long ClientId { get; set; }
        public long LegalOfficerId { get; set; }
        public DateTime AppoinmentDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? AppoinmentStatusConfigId { get; set; }
        public int? PriorityLevelConfigId { get; set; }
        public int? MeetingTypeConfigId { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }
        public string? IsBooked { get; set; }
    }
}