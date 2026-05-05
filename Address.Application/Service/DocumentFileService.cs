using AutoMapper;
using CRM.Domain;
using Galaxy.Domain.Exceptions;
using Galaxy.Domain.Models;
using Galaxy.Dto;
using Galaxy.Utility;
using LCMS.Constants;
using LCMS.Dto;
using LCMS.Utility;
using System.Net;

namespace CRM.Application
{
    public class DocumentFileService : IDocumentFileService
    {
        protected readonly ICRMUow _imasterUow;
        protected readonly IMapper _mapper;
        protected readonly IServiceProvider _serviceProvider;

        public DocumentFileService(ICRMUow masterUow, IMapper mapper, IServiceProvider serviceProvider)
        {
            _imasterUow = masterUow;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
        }

        public virtual async Task<DocumentFileDto> GetDocumentFile(long documentFileId)
        {
            DocumentFileDto documentFileDto = new DocumentFileDto();
            if (documentFileId == 0)
                throw new BusinessException((await _imasterUow.MessageRepo.GetMessageByNo(2, "documentFile Id")).Msg, HttpStatusCode.BadRequest);
            var DocumentFile = await _imasterUow.DocumentFileRepo.GetByIdAsync(documentFileId);
            if (DocumentFile == null)
                throw new BusinessException((await _imasterUow.MessageRepo.GetMessageByNo(70)).Msg, HttpStatusCode.BadRequest);
            if (DocumentFile.Id > 0)
                DocumentFile.infoMessge = await _imasterUow.MessageRepo.GetMessageByNo(71);
            documentFileDto = _mapper.Map<DocumentFileDto>(DocumentFile);
            await _imasterUow.DocumentFileRepo.GetFileContent(documentFileDto);

            return documentFileDto;
        }

        //public virtual async Task<DocumentDto> SaveDocumentFile(DocumentDto request)
        //{

        //    if (request == null) throw new BusinessException(nameof(request));

        //    var document = _mapper.Map<Document>(request);
        //    if (string.IsNullOrEmpty(document.FileName)) return request;          
        //    document.RelativePath = request.RelativePath;      
        //    if (string.IsNullOrEmpty(document.RelativePath))
        //        return request;

        //    int fileSize;
        //    string baseFileName = Path.GetFileNameWithoutExtension(document.FileName);
        //    string fileExtension = Path.GetExtension(document.FileName);
        //    string fileName = baseFileName + fileExtension;

        //    string relativePath = FileWrapper.GetPlatFormFilePath(document.RelativePath);
        //    if (!Directory.Exists(relativePath))
        //        Directory.CreateDirectory(relativePath);


        //    string fullPath = FileWrapper.GetPlatFormFilePath(relativePath, fileName);

        //    // Ensure unique file name
        //    int count = 1;
        //    while (FileWrapper.IsFileExists(fullPath))
        //    {
        //        fileName = $"{baseFileName}({count}){fileExtension}";
        //        fullPath = FileWrapper.GetPlatFormFilePath(relativePath, fileName);
        //        count++;
        //    }

        //    // Final resolved file path
        //    string actualFilePath = CommonUtil.GetPlatformFilePath(Path.Combine(relativePath, fileName));

        //    if (FileWrapper.WriteFile(relativePath, fileName, document.base64FileContent, out fileSize))
        //    {
        //        document.FileName = fileName;
        //        request.FileName = fileName;
        //        document.RelativePath = relativePath;
        //        request.RelativePath = relativePath;
        //        document.FileSize = fileSize;
        //        request.FileSize = fileSize;
        //        document.FileType = document.getActualFileType;
        //        request.FileType = document.getActualFileType;

        //        await ValidateDocumentFileMandatoryFields(document);

        //        if (document.HasError)
        //            return new DocumentDto();

        //        document = document.Id > 0 ? await _imasterUow.DocumentFileRepo.UpdateDocumentFileAsync(request)
        //            : await _imasterUow.DocumentFileRepo.InsertDocumentFileAsync(document);

        //        await _imasterUow.SaveChangesAsync();
        //    }

        //    document.infoMessge = new uMessage();
        //    document.infoMessge = await _imasterUow.MessageRepo.GetMessageByNo(7001);

        //    return _mapper.Map<DocumentDto>(document);

        //}

        //public virtual async Task<string> ConstructDocumentFilePath(string FolderName, string SubFolderName)
        //{
        //    string attachmentPath = await _imasterUow.ConfigMetaDataRepo.GetConfigMetadataValueByIdAndConstantAndMetadataName(LoanApplicationConstants.AttachmentPath.AttacthMentPathID,
        //        LoanApplicationConstants.AttachmentPath.AttachmentFolder, LoanApplicationConstants.AttachmentPath.AttachmentFolderMetadataName);
        //    if (string.IsNullOrWhiteSpace(attachmentPath)) return string.Empty;

        //    return Path.Combine(attachmentPath, FolderName, SubFolderName, "DocumentFile");
        //}

        //private async Task ValidateDocumentFileMandatoryFields(Document document)
        //{
        //    await AddErrorIf(() => string.IsNullOrWhiteSpace(document.FileType), "File Type");
        //    await AddErrorIf(() => string.IsNullOrWhiteSpace(document.FileName), "File Name");
        //    await AddErrorIf(() => string.IsNullOrWhiteSpace(document.RelativePath), "Relative Path");
        //    await AddErrorIf(() => (document.FileSize == 0.0M), "File Size");

        //    async Task AddErrorIf(Func<bool> condition, string fieldName)
        //    {
        //        if (condition())
        //            throw new BusinessException(await _imasterUow.MessageRepo.GetMessageByNo(2, fieldName), HttpStatusCode.BadRequest);
        //    }
        //}



        public virtual async Task<DocumentFileDto> SaveDocumentFile(DocumentFileDto request)
        {

            if (request == null) throw new BusinessException(nameof(request));

            var document = _mapper.Map<Document>(request);
            if (string.IsNullOrEmpty(document.FileName)) return request;

            if (!string.IsNullOrWhiteSpace(request.FolderName) && !string.IsNullOrWhiteSpace(request.SubFolderName))
            {
                document.RelativePath = await ConstructDocumentFilePath(request.FolderName, request.SubFolderName);

            }
            else
            {
                document.RelativePath = request.RelativePath;

            }

            if (string.IsNullOrEmpty(document.RelativePath))
                return request;

            int fileSize;
            string baseFileName = Path.GetFileNameWithoutExtension(document.FileName);
            string fileExtension = Path.GetExtension(document.FileName);
            string fileName = baseFileName + fileExtension;

            string relativePath = FileWrapper.GetPlatFormFilePath(document.RelativePath);
            if (!Directory.Exists(relativePath))
                Directory.CreateDirectory(relativePath);


            string fullPath = FileWrapper.GetPlatFormFilePath(relativePath, fileName);

            // Ensure unique file name
            int count = 1;
            while (FileWrapper.IsFileExists(fullPath))
            {
                fileName = $"{baseFileName}({count}){fileExtension}";
                fullPath = FileWrapper.GetPlatFormFilePath(relativePath, fileName);
                count++;
            }

            // Final resolved file path
            string actualFilePath = CommonUtil.GetPlatformFilePath(Path.Combine(relativePath, fileName));

            if (FileWrapper.WriteFile(relativePath, fileName, document.base64FileContent, out fileSize))
            {
                document.FileName = fileName;
                request.FileName = fileName;
                document.RelativePath = relativePath;
                request.RelativePath = relativePath;
                document.FileSize = fileSize;
                request.FileSize = fileSize;
                document.FileType = document.getActualFileType;
                request.FileType = document.getActualFileType;

                await ValidateDocumentFileMandatoryFields(document);

                if (document.HasError)
                    return new DocumentFileDto();

                document = document.Id > 0 ? await _imasterUow.DocumentFileRepo.UpdateDocumentFileAsync(request)
                    : await _imasterUow.DocumentFileRepo.InsertDocumentFileAsync(document);

                await _imasterUow.SaveChangesAsync();
            }

            document.infoMessge = new uMessage();
            document.infoMessge = await _imasterUow.MessageRepo.GetMessageByNo(7001);

            return _mapper.Map<DocumentFileDto>(document);

        }

        public virtual async Task<string> ConstructDocumentFilePath(string FolderName, string SubFolderName)
        {
            string attachmentPath = await _imasterUow.ConfigMetaDataRepo.GetConfigMetadataValueByIdAndConstantAndMetadataName(CRMConstants.AttachmentPath.AttacthMentPathID,
                CRMConstants.AttachmentPath.AttachmentFolder, CRMConstants.AttachmentPath.AttachmentFolderMetadataName);
            if (string.IsNullOrWhiteSpace(attachmentPath)) return string.Empty;

            return Path.Combine(attachmentPath, FolderName, SubFolderName, "DocumentFile");
        }

        private async Task ValidateDocumentFileMandatoryFields(Document document)
        {
            await AddErrorIf(() => string.IsNullOrWhiteSpace(document.FileType), "File Type");
            await AddErrorIf(() => string.IsNullOrWhiteSpace(document.FileName), "File Name");
            await AddErrorIf(() => string.IsNullOrWhiteSpace(document.RelativePath), "Relative Path");
            await AddErrorIf(() => (document.FileSize == 0.0M), "File Size");

            async Task AddErrorIf(Func<bool> condition, string fieldName)
            {
                if (condition())
                    throw new BusinessException(await _imasterUow.MessageRepo.GetMessageByNo(2, fieldName), HttpStatusCode.BadRequest);
            }
        }

        public virtual async Task<SuccessResponse> DeleteDocumentFile(long documentId)
        {
            if (documentId == 0)
            {
                var message = await _imasterUow.MessageRepo.GetMessageByNo(2, "Document Id");
                throw new BusinessException(message.Msg, HttpStatusCode.BadRequest);
            }
            var Document = await _imasterUow.DocumentFileRepo.GetDocumentFileById(documentId, true);
            SuccessResponse successResponse = new SuccessResponse();
            if (Document != null)
            {
                _imasterUow.DocumentFileRepo.Delete(Document);
                await _imasterUow.SaveChangesAsync();

                FileWrapper.DeleteFileFromDirectory(Document.RelativePath, Document.FileName);

                var Message = await _imasterUow.MessageRepo.GetMessageByNo(3);

                successResponse.Msg = new AppMessage();
                successResponse.Msg.InfoMessage = new uMessageDto();
                successResponse.Msg.InfoMessage = _mapper.Map<uMessageDto>(Message);

                return successResponse;
            }

            return successResponse;
        }
    }
}
