using Galaxy.Dto;

namespace LCMS.Dto
{
    public class LegalOfficerSchedulesDto
    {
        public long Id { get; set; }
        public long LegalOfficerId { get; set; }
        public int DayOffWeek { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public int Version { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }
}

