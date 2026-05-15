namespace CRM.Domain
{
    public class BlockedDateSearchViewModel
    {
        public long? LegalOfficerBlockedDateId { get; set; }
        public long LegalOfficerId { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? Type { get; set; }
        public string? FromTime { get; set; }
        public string? ToTime { get; set; }
        public string? Reason { get; set; }
    }
}
