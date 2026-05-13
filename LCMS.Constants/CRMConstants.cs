namespace LCMS.Constants
{
    public class CRMConstants
    {
        public static class CRMClientReferenceNumber
        {
            public const int Id = 1034;
            public const string Constant = "CUSRN";
            public const string MetadataName = "CRMClientReferenceNumber";
            public const string GenerateApplicationRefNo = "GenerateApplicationRefNo";
            public const string Prefix = "PrefixAliceName";
            public const int ErrorGeneratingRefNo = 1002;
        }

        public static class StoredProcedures
        {
            public const string APP_SP_GetClientRefNumber = "APP_SP_GetClientRefNumber";
            public const string APP_SP_GetAppoinmentRefNumber = "APP_SP_GetAppoinmentRefNumber";
            public const string APP_SP_GetAppoinmentCalendarByMonth = "APP_SP_GetAppoinmentCalendarByMonth";

        }

        public static class AttachmentPath
        {
            public const int AttacthMentPathID = 1048;
            public const string AttachmentFolder = "ATFOL";
            public const string AttachmentFolderMetadataName = "AttachmentFolder";
        }

        public static class GroupName
        {
            public const string CSO = "Customer Service Officer";
            public const string LAO = "Legal Administration Team";
        }

        public class AppoinmentReferenceNumber
        {
            public const int ErrorGeneratingRefNo = 2001;
        }

    }
}
