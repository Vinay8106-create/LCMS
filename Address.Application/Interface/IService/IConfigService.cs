using Galaxy.Dto;

using LCMS.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRM.Application

{

    public interface IConfigService

    {
        Task<ConfigDto> CreateConfig();
        Task<ConfigDto> SaveConfig(ConfigDto Config);
        Task<ConfigDto> GetConfig(long Id);
        Task<SuccessResponse> DeleteConfig(long Id);
        Task<ConfigDto> UpdateConfig(long destinationConfigId, ConfigDto SourceConfig);

    }

}

