using AutoMapper;
using CRM.Domain;
using Galaxy.Application.Mapper;
using LCMS.Dto;

namespace CRM.Application
{
    public class CRMProfile : Profile
    {
        public CRMProfile()
        {
            CreateMap<CRMClient, CRMClientDto>()
                .ForMember(d => d.Message, o => o.MapFrom<AppMessageResolver>())
                .ReverseMap();

            CreateMap<CRMClientService, CRMClientServiceDto>()
             .ForMember(d => d.Message, o => o.MapFrom<AppMessageResolver>())
             .ReverseMap();

            CreateMap<Address, AddressDto>()
                .ForMember(d => d.Msg, o => o.MapFrom<AppMessageResolver>())
                .ReverseMap();


            CreateMap<Document, DocumentFileDto>()
                .ForMember(d => d.Version, o => o.Ignore())
                .ForMember(d => d.Msg, o => o.MapFrom<AppMessageResolver>())
                .ReverseMap()
                .ForMember(d => d.Version, o => o.Ignore());

            CreateMap<CRMClientContact, CRMClientContactDto>()
            .ForMember(d => d.Msg, o => o.MapFrom<AppMessageResolver>())
            .ForMember(d => d.ResidentialAddress, o => o.Ignore())
            .ForMember(d => d.CommunicationAddress, o => o.Ignore())
            .ReverseMap();

            CreateMap<CRMClientDocument, CRMClientDocumentDto>()
                .ForMember(d => d.Msg, o => o.MapFrom<AppMessageResolver>())
                .ForMember(dest => dest.DocumentFile, opt => opt.MapFrom(src => src.Document))
                .ReverseMap();

            CreateMap<Document, DocumentFileDto>();

            CreateMap<config_ClientType, ConfigDto>()
                .ForMember(d => d.Msg, o => o.MapFrom<AppMessageResolver>())
                .ReverseMap();

            CreateMap<LegalOfficer, LegalOfficerDto>()
                .ForMember(d => d.Message, o => o.MapFrom<AppMessageResolver>())
                .ReverseMap();

            CreateMap<LegalOfficerSchedules, LegalOfficerSchedulesDto>()
               .ForMember(d => d.Message, o => o.MapFrom<AppMessageResolver>())
               .ReverseMap();

            CreateMap<LegalOfficerAppoinmentSlots, LegalOfficerAppoinmentSlotsDto>()
               .ForMember(d => d.Message, o => o.MapFrom<AppMessageResolver>())
               .ReverseMap();

            //CreateMap<LegalOfficerAppoinment, LegalOfficerAppoinmentDto>()
            //   .ForMember(d => d.Message, o => o.MapFrom<AppMessageResolver>())
            //   .ReverseMap();

            CreateMap<LegalOfficerAppoinment, LegalOfficerAppoinmentDto>()
                .ForMember(d => d.Message, o => o.MapFrom<AppMessageResolver>())
                .ForMember(d => d.AppoinmentStatusDescription, o => o.Ignore())
                .ForMember(d => d.PriorityLevelDescription, o => o.Ignore())
                .ForMember(d => d.MeetingTypeDescription, o => o.Ignore())
                .ReverseMap();

            CreateMap<LegalOfficerBlockedDates, LegalOfficerBlockedDatesDto>()
               .ForMember(d => d.Message, o => o.MapFrom<AppMessageResolver>())
               .ReverseMap();

            CreateMap<LegalOfficerAppoinment, AppoinmentTimeSlotsDto>()
                .ForMember(d => d.AppoinmentDate, o => o.MapFrom(s =>
                    s.AppoinmentDate.ToString("yyyy-MM-dd")))
                .ForMember(d => d.StartTime, o => o.MapFrom(s =>
                    s.StartTime.HasValue
                    ? s.StartTime.Value.ToString(@"hh\:mm")
                    : null))
                .ForMember(d => d.EndTime, o => o.MapFrom(s =>
                    s.EndTime.HasValue
                    ? s.EndTime.Value.ToString(@"hh\:mm")
                    : null))
                .ForMember(d => d.AppoinmentStatusConfigId, o => o.MapFrom(s =>
                    s.AppoinmentStatusConfigId))

                .ForMember(d => d.PriorityLevelConfigId, o => o.MapFrom(s =>
                    s.PriorityLevelConfigId))

                .ForMember(d => d.AppoinmentStatusDescription, o => o.Ignore())
                .ForMember(d => d.PriorityLevelDescription, o => o.Ignore())
                .ForMember(d => d.Message, o => o.MapFrom<AppMessageResolver>())
                .ReverseMap();
        }
    }
}

