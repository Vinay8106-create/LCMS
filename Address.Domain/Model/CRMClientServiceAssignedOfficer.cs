using CRM.Domain;
using Galaxy.Domain.Models;
using LCMS.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCMS.Domain
{
    public class CRMClientServiceAssignedOfficer : BaseEntity
    {
        public long Id { get; set; }
        public long ClientServiceId { get; set; }
        public string AssignedTo { get; set; } = null!;
        public string AssignedBy { get; set; } = null!;
        public DateTime AssignedDate { get; set; }
        public int StatusConfigId { get; set; }        
      
        public virtual CRMClientService? CRMClientService { get; set; }
        public virtual User? AssignedByFullName { get; set; }
        public virtual User? AssignedToFullName { get; set; }
      
    }
}
