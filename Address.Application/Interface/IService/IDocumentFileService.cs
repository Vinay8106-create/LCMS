using LCMS.Dto;

namespace CRM.Application
{
    public interface IDocumentFileService
    {
        Task<DocumentFileDto> SaveDocumentFile(DocumentFileDto request);
        Task<DocumentFileDto> GetDocumentFile(long documentFileId);
        Task<SuccessResponse> DeleteDocumentFile(long documentFileId);
        //Task<DocumentFileDto> UpdateDocumentFile(long documentFileId, DocumentFileDto updatingDocumentFile);
    }
}