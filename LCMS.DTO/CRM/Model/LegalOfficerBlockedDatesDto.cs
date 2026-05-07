using Galaxy.Dto;

namespace LCMS.Dto
{
    public class LegalOfficerBlockedDatesDto
    {
        public long Id { get; set; }
        public long LegalOfficerId { get; set; }
        public string BlockDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? Reason { get; set; }
        public int Version { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }
}

