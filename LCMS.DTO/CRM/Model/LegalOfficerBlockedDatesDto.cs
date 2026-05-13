using Galaxy.Dto;

namespace LCMS.Dto
{
    public class LegalOfficerBlockedDatesDto
    {
        public long Id { get; set; }
        public long LegalOfficerId { get; set; }
        public DateOnly BlockDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? Reason { get; set; }
        public int? BlockTypeConfigId { get; set; }
        public string? BlockTypeDescription { get; set; }
        public int Version { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }
}

