using Galaxy.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Domain
{
    public class CRMClient : BaseEntity
    {
        public long Id { get; set; }
        public string? RefNo { get; set; }
        public string? OrgName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public int? ClientTypeConfigId { get; set; }
        public int? ClientSubTypeConfigId { get; set; }
        public int? StatusConfigId { get; set; }
        public string ContactNo { get; set; }
        public string? EmailId { get; set; }
        public string? TIN { get; set; }
        public long? ResidentialAddressId { get; set; }
        public long? CommunicationAddressId { get; set; }
        public int? GenderConfigId { get; set; }
        public int? MaritalStatusConfigId { get; set; }
        public string? Occupation { get; set; }
        public string? HighestQualification { get; set; }
        public long? OfficeAddressId { get; set; }
        public long? PhotoId { get; set; }


        public virtual Address? ResidentialAddress { get; set; }
        public virtual Address? CommunicationAddress { get; set; }
        [NotMapped]
        public virtual Document? Photo { get; set; }
        //public virtual Collection<CRMClientContact>? CRMClientContacts { get; set; }
        //public virtual Collection<CRMClientDocument>? CRMClientDocuments { get; set; }

        public void ValidateMandatoryFields()
        {
            errorMsgList = new List<uMessage>();
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "First Name is mandatory." });
            }

            if (string.IsNullOrWhiteSpace(ContactNo))
            {
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "ContactNo is mandatory." });
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "Last Name is mandatory." });
            }
            if (string.IsNullOrWhiteSpace(EmailId))
            {
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "Email Id is mandatory." });
            }
        }
        
    }
}