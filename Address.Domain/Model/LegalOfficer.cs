using Galaxy.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Domain
{
    public class LegalOfficer : BaseEntity
    {
        public long Id { get; set; }
        public long UserSerialId { get; set; }
        public string RegNumber { get; set; }
        public int? DesignationConfigId { get; set; }
        public int? SpecializationConfigId { get; set; }
        public int? ExpYears { get; set; }
        public int? StatusConfigId { get; set; }
        public long? PhotoId { get; set; }
        public int? IDTypeConfigId { get; set; }
        public long IDDocId { get; set; }
              
        [NotMapped]
        public virtual Document? Photo { get; set; }
    

        public void ValidateMandatoryFieldsForLegalOfficer()
        {
            errorMsgList = new List<uMessage>();
            if (string.IsNullOrWhiteSpace(RegNumber))
            {
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "Registration Number is mandatory." });
            }          
        }

    }
}