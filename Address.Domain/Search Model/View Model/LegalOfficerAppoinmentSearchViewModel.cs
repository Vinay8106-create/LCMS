namespace CRM.Domain
{
    public class LegalOfficerAppoinmentSearchViewModel
    {
        public long? LegalOfficerAppoinmentId { get; set; }
        public string? AppointmentNo { get; set; }
        public string? ClientName { get; set; }
        public string? LegalOfficerName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? Notes { get; set; }
        public int? AppoinmentStatusConfigId { get; set; }
        public string? AppoinmentStatus { get; set; }
    }
}
