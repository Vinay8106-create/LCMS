namespace CRM.Domain
{
    public class LegalOfficerSearchViewModel
    {
        public long LegalOfficerId { get; set; }
        public string? Officer { get; set; }
        public string? RegNo { get; set; }
        public string? Designation { get; set; }
        public string? Specialization { get; set; }
        public string? LegalOfficerStatus { get; set; }
        public int? DesignationConfigId { get; set; }
        public int? SpecializationConfigId { get; set; }
        public int? LegalOfficerStatusConfigId { get; set; }
        public int ExpYears { get; set; }
    }
}
