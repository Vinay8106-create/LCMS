using Galaxy.Application;

namespace CRM.Application
{
    public interface ICRMUow : IUow
    {
        IConfigRepo ConfigRepo { get; }
        IAddressRepo AddressRepo { get; }
        IDocumentFileRepo DocumentFileRepo { get; }
        ICRMClientRepo CRMClientRepo { get; }
        ICRMClientContactRepo CRMClientContactRepo { get; }
        ICRMClientDocumentRepo CRMClientDocumentRepo { get; }
        ICRMClientSearchRepo CRMClientSearchRepo { get; }
        ICRMClientServiceRepo CRMClientServiceRepo { get; }
        ICRMClientServiceSearchRepo CRMClientServiceSearchRepo { get; }
        ILegalOfficerRepo LegalOfficerRepo { get; }

        ILegalOfficerScheduleRepo LegalOfficerScheduleRepo { get; }
        ILegalOfficerBlockDateRepo LegalOfficerBlockDateRepo { get; }
        ILegalOfficerAppoinmentRepo LegalOfficerAppoinmentRepo { get; }

        ILegalOfficerSearchRepo LegalOfficerSearchRepo { get; }
        ILegalOfficerBlockedDateSearchRepo LegalOfficerBlockedDateSearchRepo { get; }

    }
}