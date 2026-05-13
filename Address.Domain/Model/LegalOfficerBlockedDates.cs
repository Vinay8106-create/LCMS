using Galaxy.Domain.Models;

namespace CRM.Domain
{
    public class LegalOfficerBlockedDates : BaseEntity
    {
        public long Id { get; set; }
        public long LegalOfficerId { get; set; }
        public DateOnly BlockDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? Reason { get; set; }
        public int? BlockTypeConfigId { get; set; }

        public void ValidateMandatoryFieldsForLegalOfficerBlockDate()
        {
            errorMsgList = new List<uMessage>();
            if (string.IsNullOrWhiteSpace(BlockDate.ToString()))
            {
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "Date is mandatory." });
            }
            if (string.IsNullOrWhiteSpace(StartTime.ToString()))
            {
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "Start Time is mandatory." });
            }
            if (string.IsNullOrWhiteSpace(EndTime.ToString()))
            {
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "End Time is mandatory." });
            }

        }

    }
}