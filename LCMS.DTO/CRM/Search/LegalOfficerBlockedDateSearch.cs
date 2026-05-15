using Galaxy.Dto;

namespace LCMS.DTO
{
    public class LegalOfficerBlockedDateSearchDto : BaseSearch
    {
        [FilterMapping("BlockedDateViewModel.LegalOfficerId", Operation = FilterOperation.Equal)]
        public long? LegalOfficerId { get; set; }

        [FilterMapping("BlockedDateViewModel.FromDate", Operation = FilterOperation.Contains)]
        public string? FromDate { get; set; }

        [FilterMapping("BlockedDateViewModel.ToDate", Operation = FilterOperation.Contains)]
        public string? ToDate { get; set; }

        [FilterMapping("BlockedDateViewModel.Type", Operation = FilterOperation.Contains)]
        public string? Type { get; set; }
    }

    public class LegalOfficerBlockedDateSearchResultsDto : BaseSearch
    {
        [SortableField("BlockedDateViewModel.LegalOfficerBlockedDateId", IsDefaultSort = true)]
        public long? LegalOfficerBlockedDateId { get; set; }

        [SortableField("BlockedDateViewModel.FromDate")]
        public string? FromDate { get; set; }

        [SortableField("BlockedDateViewModel.Type")]
        public string? Type { get; set; }

        [SortableField("BlockedDateViewModel.FromTime")]
        public string? FromTime { get; set; }

        [SortableField("BlockedDateViewModel.ToTime")]
        public string? ToTime { get; set; }

        [SortableField("BlockedDateViewModel.Reason")]
        public string? Reason { get; set; }
    }
}
