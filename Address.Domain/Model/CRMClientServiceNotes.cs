using CRM.Domain;
using Galaxy.Domain.Models;
using LCMS.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCMS.Domain
{
    public class CRMClientServiceNotes : BaseEntity
    {
        public long Id { get; set; }
        public long ClientServiceId { get; set; }
        public string Notes { get; set; } = null!;
        public string EnteredBy { get; set; } = null!;
        public DateTime EnteredOn { get; set; }
        public int StatusConfigId { get; set; }         
        public virtual CRMClientService? CRMClientService { get; set; }
        public virtual User? EnteredByFullName { get; set; }
       
    }
}
