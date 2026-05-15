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
        public int? LegalStatusConfigId { get; set; }
        public long? PhotoId { get; set; }
        public int? IDTypeConfigId { get; set; }
        public long? IDDocId { get; set; }
        public string? Designation { get; set; }
        public string? Specialization { get; set; }
        public string? Status { get; set; }
        public string? IDType { get; set; }
        public string? EmailId { get; set; }
        public string? ContactNo { get; set; }
        public string? Qualification { get; set; }
        public int? StatusConfigId { get; set; }
        public string? Name { get; set; }
        public long? ResidentialAddressId { get; set; }
        public DocumentFileDto? Photo { get; set; } = new DocumentFileDto();

        public DocumentFileDto? Doc { get; set; } = new DocumentFileDto();
        public int Version { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();

        public AddressDto? ResidentialAddress { get; set; } = new AddressDto();
    }
}

