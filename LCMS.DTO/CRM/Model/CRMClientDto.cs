using Galaxy.Dto;

namespace LCMS.Dto
{
    public class CRMClientDto
    {
        public long Id { get; set; }
        public string? RefNo { get; set; }
        public string? OrgName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public int? ClientTypeConfigId { get; set; }
        public int? ClientSubTypeConfigId { get; set; }
        public int? StatusConfigId { get; set; }
        public string ContactNo { get; set; }
        public string? EmailId { get; set; }
        public string? TIN { get; set; }
        public long? ResidentialAddressId { get; set; }
        public long? CommunicationAddressId { get; set; }
        public int? GenderConfigId { get; set; }
        public int? MaritalStatusConfigId { get; set; }
        public string? Occupation { get; set; }
        public string? HighestQualification { get; set; }
        public long? OfficeAddressId { get; set; }
        public long? PhotoId { get; set; }
        public string? ClientType { get; set; }
        public string? ClientSubType { get; set; }
        public string? Status { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }

        public AddressDto? ResidentialAddress { get; set; } = new AddressDto();
        public AddressDto? CommunicationAddress { get; set; } = new AddressDto();
        public DocumentFileDto? Photo { get; set; } = new DocumentFileDto();
        public int Version { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }

    public class CRMClientContactDto
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? ContactNo { get; set; }
        public string? EmailId { get; set; }
        public long? ResidentialAddressId { get; set; }
        public long? CommunicationAddressId { get; set; }
        public int? GenderConfigId { get; set; }
        public int? MaritalStatusConfigId { get; set; }
        public string? Occupation { get; set; }
        public string? HighestQualification { get; set; }
        public long? OfficeAddressId { get; set; }
        public long? PhotoId { get; set; }
        public long? configRelationshipId { get; set; }
        public int? StatusConfigId { get; set; }
        public AddressDto? ResidentialAddress { get; set; } = new AddressDto();
        public AddressDto? CommunicationAddress { get; set; } = new AddressDto();
        public int Version { get; set; }
        public AppMessage? Msg { get; set; } = new AppMessage();
    }

    public class CRMClientDocumentDto
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public long? DocumentMasterId { get; set; }
        public long? DocumentId { get; set; }
        public int Version { get; set; }
        public AppMessage? Msg { get; set; } = new AppMessage();

    }
}

