using CRM.Domain;
using Galaxy.Domain.Models;
using Galaxy.Infra;
using Galaxy.Infra.EntityConfig;
using Galaxy.Workflow.Runtime.Infra;
using Galaxy.Workflow.Template.Infra;
using Microsoft.EntityFrameworkCore;

namespace LCMS.Persistence
{
    public class LCMSDbContext : ITGDbContext
    {
        public LCMSDbContext(DbContextOptions<LCMSDbContext> options, IServiceProvider serviceProvider) : base(options, serviceProvider)
        {
        }

        #region CRM 
        public DbSet<CRMClientSearchViewModel> CRMClientSearch { get; set; }
        public DbSet<CRMClientServiceSearchViewModel> CRMClientServiceSearch { get; set; }
        public DbSet<CRMClient> CRMClient { get; set; }
        public DbSet<CRMClientContact> CRMClientContact { get; set; }
        public DbSet<CRMClientDocument> CRMClientDocument { get; set; }
        public DbSet<CRMClientService> CRMClientService { get; set; }
        #endregion

        #region Config
        public DbSet<config_ClientType> config_ClientType { get; set; }  ///
        public DbSet<config_ClientSubType> config_ClientSubType { get; set; }///
        public DbSet<config_ClientStatus> config_ClientStatus { get; set; }///
        public DbSet<config_Gender> config_Gender { get; set; }///
        public DbSet<config_MaritalStatus> config_MaritalStatus { get; set; }//
        public DbSet<config_Relationship> config_Relationship { get; set; }///
        public DbSet<config_DocumentMaster> config_DocumentMaster { get; set; }
        public DbSet<config_AddressLevel1> config_AddressLevel1 { get; set; }
        public DbSet<config_AddressLevel2> config_AddressLevel2 { get; set; }
        public DbSet<config_AddressLevel3> config_AddressLevel3 { get; set; }

        public DbSet<config_Service> config_Service { get; set; }
        public DbSet<config_MatterType> config_MatterType { get; set; }
        public DbSet<config_MatterSubType> config_MatterSubType { get; set; }
        public DbSet<config_ContactMode> config_ContactMode { get; set; }
        public DbSet<config_ServiceStatus> config_ServiceStatus { get; set; }

        public DbSet<config_Designation> config_Designation { get; set; }
        public DbSet<config_Specialization> config_Specialization { get; set; }
        public DbSet<config_LegalOfficerStatus> config_LegalOfficerStatus { get; set; }
        public DbSet<config_IDType> config_IDType { get; set; }

        #endregion

        #region Legal Officer
        public DbSet<LegalOfficer> LegalOfficer { get; set; }
        public DbSet<LegalOfficerSchedules> LegalOfficerSchedules { get; set; }
        public DbSet<LegalOfficerAppoinmentSlots> LegalOfficerAppoinmentSlots { get; set; }
        public DbSet<LegalOfficerAppoinment> LegalOfficerAppoinment { get; set; }
      
        #endregion


        public override void RegisterEntityForModelCreation(ModelBuilder modelBuilder)
        {
            CRMModelRegistrar.RegisterEntityForModelCreation(modelBuilder);


            // Galaxy Framework Registrations below 3
            RegisterUserConfigurationEntityForModelCreation(modelBuilder);
            modelBuilder.RegisterWorkflowTemplateEntityForModelCreation();
            modelBuilder.RegisterWorkflowRuntimeEntityForModelCreation();
            // Upto Above

            base.RegisterEntityForModelCreation(modelBuilder);
        }

        // this are 
        public virtual void RegisterUserConfigurationEntityForModelCreation(ModelBuilder modelBuilder)
        {
            new UserConfiguration(configuration).Configure(modelBuilder.Entity<User>());
        }
    }
}
