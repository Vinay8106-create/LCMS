using Galaxy.Dto;
using LCMS.Dto;

namespace CRM.Application
{
    public interface ICRMClientService
    {
        #region Client
        Task<CRMClientSearchDto> GetClientSearchAsync();
        Task<SearchResult<CRMClientSearchResultsDto>> SearchCRMClient(CRMClientSearchDto request);
        Task<DDLData> GetClientInitialDataAsync();
        Task<CRMClientDto> GetClientByClientIdAsync(long clientId);
        Task<CRMClientDto> CreateClientAsync();
        Task<CRMClientDto> SaveClientAsync(CRMClientDto request);

        Task<CRMClientContactDto> GetClientContactByClientContactId(long clientcontactId);
        Task<CRMClientDocumentDto> CreateClientDocument();
        Task<CRMClientDocumentDto> SaveClientDocumentAsync(CRMClientDocumentDto request);
        Task<CRMClientDto> GetClientByClientRefNo(string ClientRefNo);
        #endregion

        #region Client Details
        Task<CRMClientContactDto> CreateClientContactAsync();
        Task<CRMClientContactDto> SaveClientContactAsync(CRMClientContactDto request);
        Task<CRMClientContactDto> GetClientContactByClientContactIdAsync(long clientContactId);
        Task<SuccessResponse> DeleteClientContact(long clientContactId);
        #endregion

        #region Client Service
        Task<DDLData> GetClientServiceInitialDataAsync();
        #endregion
    }
}
