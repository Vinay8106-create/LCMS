using Galaxy.Dto;

namespace LCMS.Dto
{
    public class CRMClientSearchDto : BaseSearch
    {
        [FilterMapping("ClientViewModel.ClientRefNo", Operation = FilterOperation.Contains)]
        public string? ClientRefNo { get; set; }

        [FilterMapping("ClientViewModel.ClientName", Operation = FilterOperation.Contains)]
        public string? ClientName { get; set; }

        [FilterMapping("ClientViewModel.ClientType", Operation = FilterOperation.Contains)]
        public string? Type { get; set; }

        [FilterMapping("ClientViewModel.ClientSubType", Operation = FilterOperation.Contains)]
        public string? Priority { get; set; }

        [FilterMapping("ClientViewModel.ContactNo", Operation = FilterOperation.Contains)]
        public string? ContactNo { get; set; }

        [FilterMapping("ClientViewModel.EmailId", Operation = FilterOperation.Contains)]
        public string? EmailId { get; set; }

        [FilterMapping("ClientViewModel.ClientStatus", Operation = FilterOperation.Contains)]
        public string? Status { get; set; }
    }

    public class CRMClientSearchResultsDto
    {
        [SortableField("ClientViewModel.ClientId", IsDefaultSort = true)]
        public long ClientId { get; set; }

        [SortableField("ClientViewModel.ClientRefNo")]
        public string? ClientRefNo { get; set; }

        [SortableField("ClientViewModel.ClientName")]
        public string? ClientName { get; set; }

        [SortableField("ClientViewModel.ClientType")]
        public string? Type { get; set; }

        [SortableField("ClientViewModel.ClientSubType")]
        public string? Priority { get; set; }

        [SortableField("ClientViewModel.ContactNo")]
        public string? ContactNo { get; set; }

        [SortableField("ClientViewModel.EmailId")]
        public string? EmailId { get; set; }

        [SortableField("ClientViewModel.ClientStatus")]
        public string? Status { get; set; }
    }
}

