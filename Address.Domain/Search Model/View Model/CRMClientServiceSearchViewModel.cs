namespace CRM.Domain
{
    public class CRMClientServiceSearchViewModel
    {
        public long ClientServiceId { get; set; }
        public string? ClientServiceRefNo { get; set; }
        public string? ClientRefNo { get; set; }
        public string? ClientName { get; set; }
        public string? ServiceType { get; set; }
        public string? ServiceSubType { get; set; }
        public string? ContactMode { get; set; }
        public string? ServiceStatus { get; set; }
        public long? ServiceTypeConfigId { get; set; }
        public long? ServiceSubTypeConfigId { get; set; }
        public long? ContactModeConfigId { get; set; }
        public long? ServiceStatusConfigId { get; set; }

        public DateTime? EnteredOn { get; set; }
    }
}