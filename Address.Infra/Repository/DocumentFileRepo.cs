using AutoMapper;
using CRM.Application;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Infra;
using LCMS.Dto;
using LCMS.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace CRM.Infra
{
    public class DocumentFileRepo : Repository<Document>, IDocumentFileRepo
    {
        private readonly DbContext _dbContext;
        protected readonly IServiceProvider _serviceProvider;
        public DocumentFileRepo(DbContext dbContext, IServiceProvider serviceProvider) : base(dbContext)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
        }

        public virtual async Task GetFileContent(DocumentFileDto document)
        {
            string filePath = FileWrapper.GetPlatFormFilePath(document.RelativePath, document.FileName);
            if (!FileWrapper.IsFileExists(filePath)) return;

            // Read file bytes
            byte[] fileBytes = FileWrapper.GetFileAsByteArrayFromFilePath(filePath);
            if (fileBytes == null || fileBytes.Length == 0) return;
            document.byteFileContent = fileBytes;
            document.base64FileContent = $"data:{document.getActualFileType};base64,{Convert.ToBase64String(fileBytes)}";
        }

        public virtual async Task<Document> InsertDocumentFileAsync(Document DocumentFile)
        {
            await AddAsync(DocumentFile);
            return DocumentFile;
        }

        public virtual async Task<Document> UpdateDocumentFileAsync(DocumentFileDto request)
        {
            var existingDocument = Query(x => x.Id == request.Id, true).FirstOrDefault();

            if (existingDocument == null)
                throw new BusinessException("Existing Document not found.Please Refresh Or Try again Later", HttpStatusCode.BadRequest);

            // Using DI
            _serviceProvider.GetRequiredService<IMapper>().Map(request, existingDocument);
            bool IsObjectChange = Update(existingDocument);

            return existingDocument;
        }

        public async Task<Document> GetDocumentFileById(long documentId, bool isTracking = false)
        {
            return await GetByIdAsync(documentId, isTracking);
        }
    }
}

