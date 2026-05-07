namespace CRM.Domain
{
    public class LegalOfficerSearchViewModel
    {
        public long LegalOfficerId { get; set; }
        public string? Officer { get; set; }
        public string? RegNo { get; set; }
        public string? Designation { get; set; }
        public string? Specialization { get; set; }
        public int ExpYears { get; set; }
    }
}
