namespace CRM.Domain
{
    public class BlockedDateSearchViewModel
    {
        public long LegalOfficerBlockedDateId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public string? Type { get; set; }
    }
}
