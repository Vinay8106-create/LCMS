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

        Task<CRMClientDocumentDto> CreateClientDocument();
        Task<CRMClientDto> GetClientByClientRefNo(string ClientRefNo);
        #endregion

        #region Client Contact
        Task<CRMClientContactDto> CreateClientContactAsync();
        Task<CRMClientContactDto> SaveClientContactAsync(CRMClientContactDto request);
        Task<CRMClientContactDto> GetClientContactByClientContactIdAsync(long clientContactId);
        Task<SuccessResponse> DeleteClientContact(long clientContactId);
        #endregion

        #region Client Service
        Task<DDLData> GetClientServiceInitialDataAsync();
        Task<CRMClientServiceDto> CreateClientServiceAsync();
        Task<CRMClientServiceDto> SaveClientServiceAsync(CRMClientServiceDto request);
        #endregion

        #region Legal Officer
        Task<DDLData> GetLegalOfficerInitialDataAsync();
        Task<LegalOfficerDto> CreateLegalOfficerAsync();
        Task<LegalOfficerDto> SaveLegalOfficerAsync(LegalOfficerDto request);
        Task<LegalOfficerDto> GetLegalOfficerByLegalOfficerIdAsync(long LegalOfficerId);
        Task<SuccessResponse> DeleteLegalOfficer(long LegalOfficerId);
        #endregion
    }
}
