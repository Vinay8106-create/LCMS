using Galaxy.Domain.Models;
using LCMS.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Domain
{
    public partial class Document : BaseEntity
    {
        public long Id { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public string? RelativePath { get; set; }
        public string? FileName { get; set; }
        public string? Content { get; set; }

        [NotMapped]
        public string base64FileContent { get; set; }
        [NotMapped]
        public byte[] byteFileContent { get; set; }
        [NotMapped]
        private string _getActualFileType { get; set; }
        [NotMapped]
        public string getActualFileType {
            get {
                if (!string.IsNullOrEmpty(FileType))
                {
                    if (FileType.Contains("/"))
                    {
                        _getActualFileType = FileType;
                    }
                    else if (FileType.Contains(FileWrapper.file_type_text))
                    {
                        _getActualFileType = FileWrapper.file_type_text;
                    }
                    else if (FileWrapper.image_extensions.Contains(FileType.ToUpperInvariant()))
                    {
                        _getActualFileType = FileWrapper.file_type_image + FileType;
                    }
                    else if (FileWrapper.image_extensions.Contains(FileType.ToUpperInvariant()))
                    {
                        _getActualFileType = FileWrapper.file_type_image + FileType;
                    }
                    else if (FileWrapper.application_extensions.Contains(FileType.ToUpperInvariant()))
                    {
                        _getActualFileType = FileWrapper.file_type_application + FileType;
                    }
                    else if (FileWrapper.excel_extensions.Contains(FileType.ToUpperInvariant()))
                    {
                        _getActualFileType = FileWrapper.file_type_excel + FileType;
                    }
                    else
                    {
                        _getActualFileType = FileType;
                    }
                }
                else
                {
                    _getActualFileType = FileType;
                }

                return _getActualFileType;
            }
        }


    }
}
