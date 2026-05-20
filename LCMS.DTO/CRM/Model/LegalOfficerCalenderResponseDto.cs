using Galaxy.Dto;

namespace LCMS.DTO
{
    public class LegalOfficerMonthlyCalendarDto
    {
        public long LegalOfficerId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int TotalDays { get; set; }
        public List<CalendarDayDto> Calendar { get; set; } = new List<CalendarDayDto>();
        public AppMessage? Message { get; set; } = new AppMessage();
    }

    public class CalendarDayDto
    {
        public DateOnly Date { get; set; }
        public string DayName { get; set; }
        public bool IsWeekend { get; set; }
        public List<CalendarAppointmentDto> Appointments { get; set; } = new List<CalendarAppointmentDto>();
        public List<CalendarBlockedDateDto> BlockedDates { get; set; } = new List<CalendarBlockedDateDto>();
    }

    public class CalendarAppointmentDto
    {
        public long Id { get; set; }
        public string? AppoinmentNo { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? Notes { get; set; }
    }

    public class CalendarBlockedDateDto
    {
        public long Id { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? Reason { get; set; }
    }
}

