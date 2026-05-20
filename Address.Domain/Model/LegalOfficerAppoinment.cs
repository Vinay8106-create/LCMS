using Galaxy.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Domain
{
    public class LegalOfficerAppoinment : BaseEntity
    {
        public long Id { get; set; }
        public string? AppoinmentNo { get; set; }
        public long ClientId { get; set; }
        public long LegalOfficerId { get; set; }
        public long ClientServiceId { get; set; }
        public DateTime AppoinmentDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? AppoinmentStatusConfigId { get; set; }
        public int? PriorityLevelConfigId { get; set; }
        public int? MeetingTypeConfigId { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }
        public string? IsBooked { get; set; }

        public virtual CRMClient? Client { get; set; }

        [NotMapped]
        public string? ClientName { get; set; }

        [NotMapped]
        public string? LegalOfficerName { get; set; }

        public void ValidateMandatoryFields()
        {

            if (ClientId <= 0)
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "ClientId is mandatory." });

            if (LegalOfficerId <= 0)
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "LegalOfficerId is mandatory." });

            if (AppoinmentDate == default)
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "AppoinmentDate is mandatory." });

        }
    }
}