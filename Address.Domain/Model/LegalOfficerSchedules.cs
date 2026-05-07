using Galaxy.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Domain
{
    public class LegalOfficerSchedules : BaseEntity
    {
        public long Id { get; set; }
        public long LegalOfficerId { get; set; }
        public int DayOffWeek { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }    
        
    }
}