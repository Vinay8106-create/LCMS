using Galaxy.Dto;

namespace LCMS.Dto
{
    public class LegalOfficerAppoinmentDto
    {
        public long Id { get; set; }
        public string? AppointmentNo { get; set; }
        public long ClientId { get; set; }
        public long LegalOfficerId { get; set; }
        public string AppoinmentDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public int? AppoinmentStatusConfigId { get; set; }
        public int? PriorityLevelConfigId { get; set; }
        public int? MeetingTypeConfigId { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }
        public string? IsBooked { get; set; }
        public string? AppoinmentStatus { get; set; }
        public string? PriorityLevel { get; set; }
        public string? MeetingType { get; set; }
        public int Version { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }
}

