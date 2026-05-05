using Galaxy.Domain.Models;

namespace CRM.Domain
{
    public class Address : BaseEntity
    {
        public long Id { get; set; }
        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? Line3 { get; set; }
        public int? Level1ConfigId { get; set; }
        public int? Level2ConfigId { get; set; }
        public int? Level3ConfigId { get; set; }

        public void ValidateMandatoryFields()
        {

            if (string.IsNullOrEmpty(Line1))
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "Line 1 is mandatory." });

            if (string.IsNullOrEmpty(Line2))
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "Line 2 is mandatory." });

        }
    }
}
