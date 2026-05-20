using Galaxy.Domain.Models;
using Galaxy.Dto;

namespace LCMS.Dto
{
    public class CRMClientServiceDto
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public long? LegalOfficerAppoinmentId { get; set; }
        public string? ServiceRefNo { get; set; }
        public long? ServiceConfigId { get; set; }
        public long? MatterTypeConfigId { get; set; }
        public long? MatterSubTypeConfigId { get; set; }
        public long? ContactModeConfigId { get; set; }
        public string? Notes { get; set; }
        public long? ServiceStatusConfigId { get; set; }
        public string? EnteredBy { get; set; }
        public DateTime? EnteredOn { get; set; }

        public virtual string? EnteredByFullName { get; set; }
        public string? ServiceType { get; set; }
        public string? MatterType { get; set; }
        public string? MatterSubType { get; set; }
        public string? ContactMode { get; set; }
        public string? ServiceStatusDescription { get; set; }
        public string? AssignedTo { get; set; }
        public LegalOfficerAppoinmentDto? LegalOfficerAppoinment { get; set; } = new LegalOfficerAppoinmentDto();
        public int Version { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }


    public class CRMClientServiceSectionDto
    {
        public List<CRMClientServiceDto>? Items { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }

    public class CRMClientServiceStatusHistoryDto
    {
        public long Id { get; set; }
        public long ClientServiceId { get; set; }
        public string ChangedBy { get; set; } = null!;
        public DateTime ChangedOn { get; set; }
        public int StatusConfigId { get; set; }
        public virtual string? Status { get; set; }
        public virtual string? ChangedByFullName { get; set; }
        public int Version { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }

    public class CRMClientServiceEmailHistoryDto
    {
        public long Id { get; set; }
        public long ClientServiceId { get; set; }
        public Guid? EmailTrackingId { get; set; }
        public int StatusConfigId { get; set; }
        public virtual CommunicationTracking CommunicationTracking { get; set; }
        public int Version { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }

    public class CRMClientServiceAssignedOfficerHistoryDto
    {
        public long Id { get; set; }
        public long ClientServiceId { get; set; }
        public string AssignedTo { get; set; } = null!;
        public string AssignedBy { get; set; } = null!;
        public DateTime AssignedDate { get; set; }
        public int StatusConfigId { get; set; }
        public virtual string? Status { get; set; }
        public virtual string? AssignedByFullName { get; set; }
        public virtual string? AssignedToFullName { get; set; }
        public int Version { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }

    public class CRMClientServiceNotesDto
    {
        public long Id { get; set; }
        public long ClientServiceId { get; set; }
        public string Notes { get; set; } = null!;
        public string EnteredBy { get; set; } = null!;
        public DateTime EnteredOn { get; set; }
        public int StatusConfigId { get; set; }
        public virtual string? EnteredByFullName { get; set; }
        public int Version { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
        public virtual string? Status { get; set; }
    }
}

