using Galaxy.Dto;
using Google.Protobuf;
using System.Data;

namespace LCMS.Dto
{
    public class NotesDetail
    {
        public long Id { get; set; }
        public long ForeignKey { get; set; }
        public string? Notes { get; set; }
        public string? EnteredBy { get; set; }
        public string? EnteredByFullName { get; set; }
        public DateTime EnteredDate { get; set; }
        public int StatusId { get; set; }
        public string? StatusValue { get; set; }
        public string? Status { get; set; }
        public bool IsReloadAssignOfficer { get; set; }
        public bool IsReloadEmail { get; set; }
        public bool IsReloadStatus { get; set; }
        public bool IsReloadParent { get; set; }
        public DDL? NotesDDL { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }

    public class NotesDetailSectionDto
    {
        public List<NotesDetail>? Items { get; set; } = new List<NotesDetail>();
        public AppMessage Message { get; set; } = new AppMessage();

    }

    public class StatusHistory
    {
        public long Id { get; set; }
        public long ForeignKey { get; set; }
        public int StatusId { get; set; }
        public string? StatusValue { get; set; }
        public string? Status { get; set; }
        public string? ChangedBy { get; set; }
        public string? ChangedByFullName { get; set; }
        public DateTime ChangedDate { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
    }

    public class StatusHistorySectionDto
    {
        public List<StatusHistory>? Items { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();

    }

    public class AssignedOfficer
    {
        public long Id { get; set; }
        public long ForeignKey { get; set; }
        public string? AssignedToTheOfficer { get; set; }
        public string? AssignedToFullName { get; set; }
        public string? AssignedByTheOfficer { get; set; }
        public string? AssignedByFullName { get; set; }
        public int StatusId { get; set; }
        public string? StatusValue { get; set; }
        public string? Status { get; set; }
        public DateTime AssignedDate { get; set; }
        public int? BranchId { get; set; }
        public string? BranchValue { get; set; }
        public string? Branch { get; set; }
        public AppMessage? Message { get; set; } = new AppMessage();
        public int Version { get; set; }
        public List<DDLClass>? UserDDLs { get; set; }

    }

    public class AssignedOfficerSectionDto
    {
        public List<AssignedOfficer>? Items { get; set; } = new List<AssignedOfficer>();
        public AppMessage? Message { get; set; } = new AppMessage();

    }

    public class EmailHistory
    {
        public long Id { get; set; }
        public long ForeignKey { get; set; }
        public Guid CommunicationTrackingId { get; set; }
        public string SentBy { get; set; }
        public string SenderEmail { get; set; }
        public string RecipientEmail { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public DateTime? SentDate { get; set; }
        public int Version { get; set; }
    }

    public class EmailHistorySectionDto
    {
        public List<EmailHistory> Items { get; set; } = new List<EmailHistory>();
        public AppMessage? Message { get; set; } = new AppMessage();

    }

    public class SuccessResponse
    {
        public AppMessage Msg { get; set; } = new AppMessage();
        public bool IsDeleted { get; set; }
        public bool IsValidated { get; set; }
        public bool IsUploaded { get; set; }
    }

    public class DocumentBase64Dto
    {
        public long DocumentId { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public AppMessage Msg { get; set; }
        public int Version { get; set; }
    }

    public class DocumentResultDto
    {
        public int FileSize { get; set; }
        public string FileType { get; set; }
        public string DocumentName { get; set; }
        public ByteString Byte { get; set; }
        public AppMessage Msg { get; set; }
    }

    public class ExcelExportRequest
    {
        public string ScreenName { get; set; }
        public string BasePath { get; set; }
        public string FolderPath { get; set; }
        public Dictionary<string, string> ColumnMappings { get; set; }
        public DataTable Data { get; set; }
    }

    public class ExcelExportResponse
    {
        public string FileName { get; set; }

        public byte[] GeneratedFile { get; set; }
    }

    public class ByteResponse
    {
        public byte[] ByteData { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public string base64FileContent { get; set; }
        public AppMessage Msg { get; set; }
    }

}
