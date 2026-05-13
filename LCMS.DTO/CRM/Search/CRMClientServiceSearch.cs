using Galaxy.Dto;

namespace LCMS.DTO
{
    public class CRMClientServiceSearchDto : BaseSearch
    {
        [FilterMapping("ClientServiceViewModel.ClientServiceRefNo", Operation = FilterOperation.Contains)]
        public string? ClientServiceRefNo { get; set; }

        [FilterMapping("ClientServiceViewModel.ClientRefNo", Operation = FilterOperation.Contains)]
        public string? ClientRefNo { get; set; }

        [FilterMapping("ClientServiceViewModel.ServiceTypeConfigId", Operation = FilterOperation.Equal)]
        public long? ServiceType { get; set; }

        [FilterMapping("ClientServiceViewModel.ServiceSubTypeConfigId", Operation = FilterOperation.Equal)]
        public long? ServiceSubType { get; set; }

        [FilterMapping("ClientServiceViewModel.ContactModeConfigId", Operation = FilterOperation.Equal)]
        public long? ContactMode { get; set; }

        [FilterMapping("ClientServiceViewModel.ServiceStatusConfigId", Operation = FilterOperation.Equal)]
        public long? ServiceStatus { get; set; }

        [FilterMapping("ClientServiceViewModel.EnteredOn", Operation = FilterOperation.GreaterThanOrEqual)]
        public DateTime? FromDate { get; set; }

        [FilterMapping("ClientServiceViewModel.EnteredOn", Operation = FilterOperation.LessThanOrEqual)]
        public DateTime? ToDate { get; set; }
    }

    public class CRMClientServiceSearchResultsDto
    {
        [SortableField("ClientServiceViewModel.ClientServiceId", IsDefaultSort = true)]
        public long ClientServiceId { get; set; }

        [SortableField("ClientServiceViewModel.ClientServiceRefNo")]
        public string? ClientServiceRefNo { get; set; }

        [SortableField("ClientServiceViewModel.ClientRefNo")]
        public string? ClientRefNo { get; set; }

        [SortableField("ClientServiceViewModel.ClientName")]
        public string? ClientName { get; set; }

        [SortableField("ClientServiceViewModel.ServiceType")]
        public string? ServiceType { get; set; }

        [SortableField("ClientServiceViewModel.ServiceSubType")]
        public string? ServiceSubType { get; set; }

        [SortableField("ClientServiceViewModel.ContactMode")]
        public string? ContactMode { get; set; }

        [SortableField("ClientServiceViewModel.ServiceStatus")]
        public string? ServiceStatus { get; set; }

        [SortableField("ClientServiceViewModel.EnteredOn")]
        public DateTime? EnteredOn { get; set; }
    }
}
