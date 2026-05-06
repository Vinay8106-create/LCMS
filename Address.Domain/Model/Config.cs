using Galaxy.Domain.Models;

namespace CRM.Domain
{
    public class config_ClientType : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_ClientSubType : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_ClientStatus : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_Gender : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_MaritalStatus : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_DocumentMaster : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_AddressLevel1 : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_AddressLevel2 : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_AddressLevel3 : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_Relationship : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_Service : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_MatterType : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_MatterSubType : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_ContactMode : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_ServiceStatus : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_Designation : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_Specialization : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }
    }

    public class config_LegalOfficerStatus : BaseEntity
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int ConfigId { get; set; }


        public class config_IDType : BaseEntity
        {
            public long Id { get; set; }
            public string Description { get; set; }
            public int ConfigId { get; set; }
        }
    }
}