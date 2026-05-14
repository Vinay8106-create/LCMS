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
        public string? AppoinmentStatusDescription { get; set; }
        public string? PriorityLevelDescription { get; set; }
        public string? MeetingTypeDescription { get; set; }

        public string? ClientName { get; set; }
        public string? LegalOfficerName { get; set; }
        public int Version { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }

    public class AppoinmentCalendarDto    // Works For Monthly Calendar View
    {
        public string AppoinmentDate { get; set; }
        public int TotalAppointments { get; set; }
        public int BookedCount { get; set; }
        public int AvailableCount { get; set; }
        public int PendingCount { get; set; }
        public string DayStatus { get; set; }  // drives color on UI
        public AppMessage? Message { get; set; } = new AppMessage();
    }

    public class AppoinmentTimeSlotsDto
    {
        public string AppoinmentDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string IsBooked { get; set; }
        public int? AppoinmentStatusConfigId { get; set; }
        public string AppoinmentStatusDescription { get; set; }
        public int? PriorityLevelConfigId { get; set; }
        public string? PriorityLevelDescription { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }
}

