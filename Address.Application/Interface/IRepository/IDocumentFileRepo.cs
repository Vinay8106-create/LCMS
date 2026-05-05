using CRM.Domain;
using Galaxy.Application;
using LCMS.Dto;

namespace CRM.Application
{
    public interface IDocumentFileRepo : IRepository<Document>
    {
        Task GetFileContent(DocumentFileDto documentFileDto);
        Task<Document> InsertDocumentFileAsync(Document documentFile);
        Task<Document> UpdateDocumentFileAsync(DocumentFileDto request);
        Task<Document> GetDocumentFileById(long documentId, bool isTracking = false);
    }
}