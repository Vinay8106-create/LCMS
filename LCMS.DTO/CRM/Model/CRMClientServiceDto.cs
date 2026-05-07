using Galaxy.Dto;

namespace LCMS.Dto
{
    public class CRMClientServiceDto
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string? ServiceRefNo { get; set; }
        public int? ServiceConfigId { get; set; }
        public int? MatterTypeConfigId { get; set; }
        public int? MatterSubTypeConfigId { get; set; }
        public int? ContactModeConfigId { get; set; }
        public string? Notes { get; set; }
        public int? ServiceStatusConfigId { get; set; }
        public string? EnteredBy { get; set; }
        public DateTime? EnteredOn { get; set; }

        public string? ServiceType { get; set; }
        public string? MatterType { get; set; }
        public string? MatterSubType { get; set; }
        public string? ContactMode { get; set; }
        public string? ServiceStatus { get; set; }
        public int Version { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }
}

