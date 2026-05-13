using Galaxy.Domain.Models;
namespace CRM.Domain
{
    public class LegalOfficerSchedules : BaseEntity
    {
        public long Id { get; set; }
        public long LegalOfficerId { get; set; }
        public int DayOffWeek { get; set; }

        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? SlotDuration { get; set; }
        public string? ISActive { get; set; }
        public TimeSpan? BreakStartTime { get; set; }
        public TimeSpan? BreakEndTime { get; set; }

        public void ValidateMandatoryFieldsForLegalOfficerSchedule()
        {
            errorMsgList = new List<uMessage>();
            if (string.IsNullOrWhiteSpace(StartTime.ToString()))
            {
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "Start Time is mandatory." });
            }
            if (string.IsNullOrWhiteSpace(EndTime.ToString()))
            {
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "End Time is mandatory." });
            }
            if (SlotDuration == 0)
            {
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "Slot Duration is mandatory." });
            }

        }

    }
}