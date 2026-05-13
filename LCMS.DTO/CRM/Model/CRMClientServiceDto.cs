using Galaxy.Dto;

namespace LCMS.Dto
{
    public class CRMClientServiceDto
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string? ServiceRefNo { get; set; }
        public long? ServiceConfigId { get; set; }
        public long? MatterTypeConfigId { get; set; }
        public long? MatterSubTypeConfigId { get; set; }
        public long? ContactModeConfigId { get; set; }
        public string? Notes { get; set; }
        public long? ServiceStatusConfigId { get; set; }
        public string? EnteredBy { get; set; }
        public DateTime? EnteredOn { get; set; }

        public string? ServiceType { get; set; }
        public string? MatterType { get; set; }
        public string? MatterSubType { get; set; }
        public string? ContactMode { get; set; }
        public string? ServiceStatusDescription { get; set; }
        public string? AssignedTo { get; set; }
        public int Version { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }


    public class CRMClientServiceSectionDto
    {
        public List<CRMClientServiceDto>? Items { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }
}

