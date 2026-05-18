using Galaxy.Dto;

namespace LCMS.DTO
{
    public class LegalOfficerAppoinmentSearchDto : BaseSearch
    {
        [FilterMapping("LegalOfficerAppoinmentSearchViewModel.AppointmentNo", Operation = FilterOperation.Contains)]
        public string? AppointmentNo { get; set; }

        [FilterMapping("LegalOfficerAppoinmentSearchViewModel.ClientName", Operation = FilterOperation.Contains)]
        public string? ClientName { get; set; }

        [FilterMapping("LegalOfficerAppoinmentSearchViewModel.LegalOfficerName", Operation = FilterOperation.Contains)]
        public string? LegalOfficerName { get; set; }

        [FilterMapping("LegalOfficerAppoinmentSearchViewModel.FromDate", Operation = FilterOperation.Contains)]
        public DateTime? FromDate { get; set; }

        [FilterMapping("LegalOfficerAppoinmentSearchViewModel.ToDate", Operation = FilterOperation.Contains)]
        public DateTime? ToDate { get; set; }

        [FilterMapping("LegalOfficerAppoinmentSearchViewModel.AppoinmentStatusConfigId", Operation = FilterOperation.Equal)]
        public int? AppoinmentStatusConfigId { get; set; }
    }

    public class LegalOfficerAppoinmentSearchResultsDto : BaseSearch
    {
        [SortableField("LegalOfficerAppoinmentSearchViewModel.LegalOfficerAppoinmentId", IsDefaultSort = true)]
        public long? LegalOfficerAppoinmentId { get; set; }

        [SortableField("LegalOfficerAppoinmentSearchViewModel.AppointmentNo")]
        public string? AppointmentNo { get; set; }

        [SortableField("LegalOfficerAppoinmentSearchViewModel.ClientName")]
        public string? ClientName { get; set; }

        [SortableField("LegalOfficerAppoinmentSearchViewModel.LegalOfficerName")]
        public string? LegalOfficerName { get; set; }

        [SortableField("LegalOfficerAppoinmentSearchViewModel.AppoinmentDate")]
        public DateTime? AppoinmentDate { get; set; }

        [SortableField("LegalOfficerAppoinmentSearchViewModel.FromDate")]
        public DateTime? FromDate { get; set; }

        [SortableField("LegalOfficerAppoinmentSearchViewModel.ToDate")]
        public DateTime? ToDate { get; set; }

        [SortableField("LegalOfficerAppoinmentSearchViewModel.StartTime")]
        public TimeSpan? StartTime { get; set; }

        [SortableField("LegalOfficerAppoinmentSearchViewModel.EndTime")]
        public TimeSpan? EndTime { get; set; }

        [SortableField("LegalOfficerAppoinmentSearchViewModel.Notes")]
        public string? Notes { get; set; }

        [SortableField("LegalOfficerAppoinmentSearchViewModel.AppoinmentStatus")]
        public string? AppoinmentStatus { get; set; }
        public int? AppoinmentStatusConfigId { get; set; }
    }
}
