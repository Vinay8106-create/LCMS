using Galaxy.Dto;

namespace LCMS.Dto
{
    public class LegalOfficerSchedulesDto
    {
        public long Id { get; set; }
        public long LegalOfficerId { get; set; }
        public int DayOffWeek { get; set; }
        public string Dayname { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public int? SlotDuration { get; set; }
        public string? ISActive { get; set; }
        public int Version { get; set; }
        public string? BreakStartTime { get; set; }
        public string? BreakEndTime { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();

        // Add this — Slot Preview result lives here
        public List<SlotDto>? SlotPreview { get; set; }
        public string? BreakTimeLabel { get; set; }
    }

    // Small Slot DTO — reusable
    public class SlotDto
    {
        public string SlotStart { get; set; }
        public string SlotEnd { get; set; }
    }
}

