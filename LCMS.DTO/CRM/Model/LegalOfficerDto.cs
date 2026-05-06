using Galaxy.Dto;

namespace LCMS.Dto
{
    public class LegalOfficerDto
    {
        public long Id { get; set; }
        public long UserSerialId { get; set; }
        public string RegNumber { get; set; }
        public int? DesignationConfigId { get; set; }
        public int? SpecializationConfigId { get; set; }
        public int? ExpYears { get; set; }
        public int? StatusConfigId { get; set; }
        public long? PhotoId { get; set; }
        public int? IDTypeConfigId { get; set; }
        public long IDDocId { get; set; }
        public string? Designation { get; set; }
        public string? Specialization { get; set; }
        public string? Status { get; set; }
        public string? IDType { get; set; }       
        public DocumentFileDto? Photo { get; set; } = new DocumentFileDto();
        public int Version { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }   
}

