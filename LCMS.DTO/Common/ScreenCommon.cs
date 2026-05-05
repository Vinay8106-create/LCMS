using Galaxy.Dto;
using ITGAdmin.DTO;

namespace LCMS.Dto;

public class AccessValidation
{
    /// <summary>
    /// Indicates whether the user has full access to the application.
    /// This is true when the application is in "Pending Submission" or "Verification" status.
    /// </summary>
    public bool HasFullAccess { get; set; }

    /// <summary>
    /// Indicates whether the user can self-assign the application.
    /// This is true when the user's role matches the application's current status role.
    /// </summary>
    public bool CanSelfAssign { get; set; }

    /// <summary>
    /// Indicates whether the user can reassign the application.
    /// This is true when the user's role matches the application's status role and the user is a higher official.
    /// </summary>
    public bool CanReassign { get; set; }

    /// <summary>
    /// Indicates whether the user can delete the application.
    /// This is true when the application is in "Pending Submission" or "Verification" status.
    /// </summary>
    public bool CanDelete { get; set; }

    /// <summary>
    /// Indicates whether the user can edit the application.
    /// This is true when the application is in "Pending Submission" or "Verification" status.
    /// </summary>
    public bool CanEdit { get; set; }

    /// <summary>
    /// Indicates whether the user has permission to move the application to another status.
    /// </summary>
    public bool CanMove { get; set; }

    /// <summary>
    /// Indicates whether the first checkbox (e.g., for verification) should be visible or active.
    /// This is true when the application is in "Pending Verification" status.
    /// </summary>
    public bool IsFirstCheckbox { get; set; }

    /// <summary>
    /// Indicates whether the second checkbox (e.g., for approval) should be visible or active.
    /// This is true when the application is in "Pending Approval" status.
    /// </summary>
    public bool IsSecondCheckbox { get; set; }

    /// <summary>
    /// Indicates whether the user can approve or reject the application.
    /// This is true when the application is in "Pending Approval" status.
    /// </summary>
    public bool CanApproveOrReject { get; set; }
}

public class UserAccessResponse
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanView { get; set; }
}
public class UserAccessRequest
{
    public List<UserRights> UserRights { get; set; } = new List<UserRights>();
    public string? ViewName { get; set; }

}
public class NewNotes
{
    public long NotesId { get; set; }
    public long ApplicationId { get; set; }
    public string CurrentStatus { get; set; }
    public string EnteredBy { get; set; }
    public DateTime? EnteredDate { get; set; }
    public DDL? StatusDDL { get; set; }
}

public class DDLCheckbox
{
    public long Id { get; set; }
    public string? Constant { get; set; }
    public string? Description { get; set; }
    public bool IsChecked { get; set; }
}
public class ApplicationCheckList
{
    public long? ApplicationId { get; set; }
    public long ApplicationChecklistId { get; set; }
    public long ChecklistConfigurationId { get; set; }
    public string? ChecklistDescription { get; set; }
    public bool? IsChecked { get; set; }
    public bool? IsSecondChecked { get; set; }
    public string? SecondCheckedBy { get; set; }
    public string? SecondCheckedByUserFullName { get; set; }
    public DateTime? SecondCheckedDateTime { get; set; }
    public string? CheckedBy { get; set; }
    public string? CheckedByUserFullName { get; set; }
    public DateTime? CheckedDateTime { get; set; }
    public ApplicationChecklistConfiguration ApplicationChecklistConfiguration { get; set; }
    public int? version { get; set; }
    public AppMessage Msg { get; set; } = new AppMessage();

}

public class ApplicationCheckLists
{
    public List<ApplicationCheckList> Items { get; set; } = new List<ApplicationCheckList>();
    public AppMessage Msg { get; set; } = new AppMessage();
}
public class ApplicationDocument
{
    public long ApplicationId { get; set; }
    public long ApplicationDocumentId { get; set; }
    public long ForeignKey { get; set; } // for nominee purpose
    public long DocumentConfigurationId { get; set; }
    public ApplicationDocumentConfiguration ApplicationDocumentConfiguration { get; set; }
    public List<ApplicationDocumentDetail> ApplicationDocumentDetails { get; set; } = new List<ApplicationDocumentDetail>();
    public AppMessage? Msg { get; set; }

}

public class ApplicationDocuments
{
    public long ApplicationId { get; set; }
    public List<ApplicationDocument> Items { get; set; } = new List<ApplicationDocument>();
    public AppMessage? Msg { get; set; }
}


public class ApplicationDocumentDetail
{
    public string? ChangedBy { get; set; }
    public long DocumentFileId { get; set; }
    public string? FileName { get; set; }
    public string? comments { get; set; }
    public string? DocumentStatusValue { get; set; }
    public string? DocumentStatusDescription { get; set; }
    public long ApplicationId { get; set; }
    public long ApplicationDocumentDetailId { get; set; }
    public long ApplicationDocumentId { get; set; }
    public string? ExistingData { get; set; }
    public string UploadedBy { get; set; }
    public DateTime? UploadedDate { get; set; }
    public DocumentFileDto? DocumentFile { get; set; } = new DocumentFileDto();
    public AppMessage? Msg { get; set; }

}

public class ApplicationDocumentConfiguration
{
    public long Id { get; set; }
    public bool? IsMultiple { get; set; }
    public string? DocumentSize { get; set; }
    public string? ApplicationTypeValue { get; set; }
    public string? ApplicationTypeDescription { get; set; }
    public bool? IsApprovalRequied { get; set; }
    public bool? IsMandatory { get; set; }
    public string? DocumentName { get; set; }
    public string? ForValue { get; set; }

}


public class ApplicationChecklistConfiguration
{
    public long Id { get; set; }
    public bool? IsMandatory { get; set; }
    public bool? IsSecondLevelCheckRequired { get; set; }
    public string? ChecklistDescription { get; set; }
}

public class FieldConfigurations
{
    public List<FieldConfigDDL> Items { get; set; }
}
public class FieldConfigDDL
{
    public long Id { get; set; }
    public string? Constant { get; set; }
    public string? Description { get; set; }

    // will add further objects...
}
