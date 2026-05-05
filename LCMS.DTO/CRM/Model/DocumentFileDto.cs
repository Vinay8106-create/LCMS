using Galaxy.Dto;

namespace LCMS.Dto
{
    public class DocumentFileDto
    {
        public long Id { get; set; }
        public string FileType { get; set; }
        public decimal FileSize { get; set; }
        public string RelativePath { get; set; }
        public string? FileName { get; set; }

        public string? Content { get; set; }
        public string FolderName { get; set; } = string.Empty;
        public string SubFolderName { get; set; } = string.Empty;
        public string? base64FileContent { get; set; }
        public byte[]? byteFileContent { get; set; }
        public string getActualFileType { get; set; }
        public int Version { get; set; }

        public AppMessage Msg { get; set; } = new AppMessage();
    }
}

