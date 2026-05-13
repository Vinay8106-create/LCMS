using CRM.Domain;
using Galaxy.Application;
using Galaxy.Dto;
using LCMS.Dto;

namespace CRM.Application
{

    public interface IConfigRepo : IRepository<config_ClientType>
    {
        Task<config_ClientType> InsertConfigAsync(config_ClientType config);
        Task<config_ClientType> UpdateConfigAsync(ConfigDto request);
        Task<config_ClientType> GetConfigById(long Id, bool isTracking = false);
        Task SetDescription<T>(T model) where T : class;
        Task SetAddressDescription<T>(T model) where T : class;
        Task<DDLData> ClientInitialData();

    }
}
