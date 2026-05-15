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
    public class CRMClientServiceStatusHistory : BaseEntity
    {
        public long Id { get; set; }
        public long ClientServiceId { get; set; }
        public string ChangedBy { get; set; } = null!;
        public DateTime ChangedOn { get; set; }
        public int StatusConfigId { get; set; }
        public virtual CRMClientService? CRMClientService { get; set; }
        public virtual User? ChangedByFullName { get; set; }
    }
}
