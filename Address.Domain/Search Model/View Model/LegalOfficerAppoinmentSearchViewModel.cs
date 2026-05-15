namespace CRM.Domain
{
    public class LegalOfficerAppoinmentSearchViewModel
    {
        public string? AppointmentNo { get; set; }
        public string? ClientName { get; set; }
        public string? LegalOfficerName { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public int? AppoinmentStatusConfigId { get; set; }
    }
}
