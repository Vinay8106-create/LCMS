using Galaxy.Dto;

namespace LCMS.DTO
{
    public class LegalOfficerSearchDto : BaseSearch
    {
        [FilterMapping("LegalOfficerViewModel.Officer", Operation = FilterOperation.Contains)]
        public string? Officer { get; set; }

        [FilterMapping("LegalOfficerViewModel.RegNo", Operation = FilterOperation.Contains)]
        public string? RegNo { get; set; }

        [FilterMapping("LegalOfficerViewModel.Designation", Operation = FilterOperation.Contains)]
        public string? Designation { get; set; }

        [FilterMapping("LegalOfficerViewModel.Specialization", Operation = FilterOperation.Contains)]
        public string? Specialization { get; set; }
    }

    public class LegalOfficerSearchResultsDto : BaseSearch
    {
        [SortableField("LegalOfficerViewModel.LegalOfficerId", IsDefaultSort = true)]
        public long LegalOfficerId { get; set; }

        [SortableField("LegalOfficerViewModel.Officer")]
        public string? Officer { get; set; }

        [SortableField("LegalOfficerViewModel.RegNo")]
        public string? RegNo { get; set; }

        [SortableField("LegalOfficerViewModel.Designation")]
        public string? Designation { get; set; }

        [SortableField("LegalOfficerViewModel.Specialization")]
        public string? Specialization { get; set; }

        [SortableField("LegalOfficerViewModel.ExpYears")]
        public int? ExpYears { get; set; }
    }
}
