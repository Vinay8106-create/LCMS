using CRM.Domain;
using Galaxy.Domain.Models;
using LCMS.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCMS.Domain
{
    public class CRMClientServiceEmailHistory : BaseEntity
    {
        public long Id { get; set; }
        public long ClientServiceId { get; set; }
        public Guid? EmailTrackingId { get; set; }
        public int StatusConfigId { get; set; }         
        public virtual CRMClientService? CRMClientService { get; set; }
        public virtual CommunicationTracking CommunicationTracking { get; set; }
    }
}
