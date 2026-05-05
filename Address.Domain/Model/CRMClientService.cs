using DocumentFormat.OpenXml.Wordprocessing;
using Galaxy.Domain.Models;
using LCMS.Domain;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Domain
{
    public class CRMClientService : BaseEntity
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string? ServiceRefNo { get; set; }
        public int ServiceConfigId { get; set; }
        public int MatterTypeConfigId { get; set; }
        public int MatterSubTypeConfigId { get; set; }
        public int ContactModeConfigId { get; set; }
        public string? Notes { get; set; }
        public int ServiceStatusConfigId { get; set; }
        public string? EnteredBy { get; set; }
        public string? EnteredOn { get; set; }
        public virtual Collection<CRMClientServiceAssignedOfficer> CRMClientServiceAssignedOfficers { get; set; }       
        public virtual Collection<CRMClientServiceNotes> CRMClientServiceNotes { get; set; }
        public virtual Collection<CRMClientServiceStatusHistory> CRMClientServiceStatusHistories { get; set; }
        public virtual Collection<CRMClientServiceEmailHistory> CRMClientServiceEmailHistories { get; set; }

        public void ValidateMandatoryFieldsForService()
        {
            errorMsgList = new List<uMessage>();
            if (ServiceConfigId==0)
            {
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "Service type is mandatory." });
            }

            if (MatterTypeConfigId==0)
            {
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "Matter Type is mandatory." });
            }

            if (MatterSubTypeConfigId==0)
            {
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "Matter Subtype is mandatory." });
            }
            if (ContactModeConfigId==0)
            {
                errorMsgList.Add(new uMessage() { MsgType = messageType.Error, Msg = "Contact Mode is mandatory." });
            }
        }

    }
}