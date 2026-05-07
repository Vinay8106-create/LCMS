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
                .ReverseMap();

            CreateMap<config_ClientType, ConfigDto>()
                .ForMember(d => d.Msg, o => o.MapFrom<AppMessageResolver>())
                .ReverseMap();
            CreateMap<LegalOfficer, LegalOfficerDto>()
                .ForMember(d => d.Message, o => o.MapFrom<AppMessageResolver>())
                .ReverseMap();

        }


    }
}

