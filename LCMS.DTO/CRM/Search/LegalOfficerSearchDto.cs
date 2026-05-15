using Galaxy.Dto;

namespace LCMS.DTO
{
    public class LegalOfficerSearchDto : BaseSearch
    {
        [FilterMapping("LegalOfficerViewModel.Officer", Operation = FilterOperation.Contains)]
        public string? Officer { get; set; }

        [FilterMapping("LegalOfficerViewModel.RegNo", Operation = FilterOperation.Contains)]
        public string? RegNo { get; set; }

        [FilterMapping("LegalOfficerViewModel.DesignationConfigId", Operation = FilterOperation.Equal)]
        public int? designationConfigId { get; set; }

        [FilterMapping("LegalOfficerViewModel.SpecializationConfigId", Operation = FilterOperation.Equal)]
        public int? specializationConfigId { get; set; }

        [FilterMapping("LegalOfficerViewModel.LegalOfficerStatusConfigId", Operation = FilterOperation.Equal)]
        public int? legalOfficerStatusConfigId { get; set; }
    }

    public class LegalOfficerSearchResultsDto
    {
        [SortableField("LegalOfficerViewModel.LegalOfficerId", IsDefaultSort = true)]
        public long LegalOfficerId { get; set; }

        [SortableField("LegalOfficerViewModel.Officer")]
        public string? Officer { get; set; }

        [SortableField("LegalOfficerViewModel.RegNo")]
        public string? RegNo { get; set; }

        [SortableField("LegalOfficerViewModel.Designation")]
        public string? Designation { get; set; }
        public int? designationConfigId { get; set; }

        [SortableField("LegalOfficerViewModel.Specialization")]
        public string? Specialization { get; set; }
        public int? specializationConfigId { get; set; }

        [SortableField("LegalOfficerViewModel.LegalOfficerStatus")]
        public string? LegalOfficerStatus { get; set; }
        public int? legalOfficerStatusConfigId { get; set; }

        [SortableField("LegalOfficerViewModel.ExpYears")]
        public int? ExpYears { get; set; }
    }
}
