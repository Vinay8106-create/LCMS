using Galaxy.Dto;

namespace LCMS.DTO
{
    public class LegalOfficerAppoinmentSearchDto : BaseSearch
    {
        [FilterMapping("LegalOfficerAppoinmentViewModel.AppoinmentNo", Operation = FilterOperation.Contains)]
        public string? AppoinmentNo { get; set; }

        [FilterMapping("LegalOfficerAppoinmentViewModel.ClientName", Operation = FilterOperation.Contains)]
        public string? ClientName { get; set; }

        [FilterMapping("LegalOfficerAppoinmentViewModel.LegalOfficerName", Operation = FilterOperation.Contains)]
        public string? LegalOfficerName { get; set; }

        [FilterMapping("LegalOfficerAppoinmentViewModel.AppoinmentDate", Operation = FilterOperation.Equal)]
        public DateOnly? AppoinmentDate { get; set; }

        [FilterMapping("LegalOfficerAppoinmentViewModel.AppoinmentStatusConfigId", Operation = FilterOperation.Equal)]
        public int? AppoinmentStatusConfigId { get; set; }
    }

    public class LegalOfficerAppoinmentSearchResultsDto : BaseSearch
    {
        [SortableField("LegalOfficerAppoinmentViewModel.LegalOfficerAppoinmentId", IsDefaultSort = true)]
        public long? LegalOfficerAppoinmentId { get; set; }

        [SortableField("LegalOfficerAppoinmentViewModel.AppoinmentNo")]
        public string? AppoinmentNo { get; set; }

        [SortableField("LegalOfficerAppoinmentViewModel.ClientName")]
        public string? ClientName { get; set; }

        [SortableField("LegalOfficerAppoinmentViewModel.LegalOfficerName")]
        public string? LegalOfficerName { get; set; }

        [SortableField("LegalOfficerAppoinmentViewModel.AppoinmentDate")]
        public DateOnly? AppoinmentDate { get; set; }

        [SortableField("LegalOfficerAppoinmentViewModel.StartTime")]
        public TimeSpan? StartTime { get; set; }

        [SortableField("LegalOfficerAppoinmentViewModel.EndTime")]
        public TimeSpan? EndTime { get; set; }

        [SortableField("LegalOfficerAppoinmentViewModel.Notes")]
        public string? Notes { get; set; }

        [SortableField("LegalOfficerAppoinmentViewModel.AppoinmentStatus")]
        public string? AppoinmentStatus { get; set; }
        public int? AppoinmentStatusConfigId { get; set; }
    }
}
