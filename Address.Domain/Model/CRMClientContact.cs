using Galaxy.Domain.Models;

namespace CRM.Domain
{
    public class CRMClientContact : BaseEntity
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? ContactNo { get; set; }
        public string? EmailId { get; set; }
        public long? ResidentialAddressId { get; set; }
        public long? CommunicationAddressId { get; set; }
        public int? GenderConfigId { get; set; }
        public int? MaritalStatusConfigId { get; set; }
        public string? Occupation { get; set; }
        public string? HighestQualification { get; set; }
        //public long? OfficeAddressId { get; set; }
        public long? configRelationshipId { get; set; }
        public int StatusConfigId { get; set; }

        public virtual Address? ResidentialAddress { get; set; }
        public virtual Address? CommunicationAddress { get; set; }

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
