namespace SPL.Masters.Api.AutoMapper
{
    using global::AutoMapper;

    using SPL.Domain.SPL.Masters;
    using SPL.Masters.Api.DTOs.Artifactdesign;
    using SPL.Masters.Api.DTOs.Configuration;
    using SPL.Masters.Infrastructure.Entities;

    public class MastersProfile : Profile
    {
        public MastersProfile()
        {
            _ = this.CreateMap<CatSidcoInformationDto, CatSidcoInformation>().ReverseMap();
            _ = this.CreateMap<CatSidcoOtherInformationDto, CatSidcoOtherInformation>().ReverseMap();

            _ = this.CreateMap<CatSidcoInformation, SplCatsidco>().ReverseMap();
            _ = this.CreateMap<CatSidcoOtherInformation, SplCatsidcoOther>().ReverseMap();

            _ = this.CreateMap<SplNormasrep, RulesRep>().ReverseMap();
            _ = this.CreateMap<RulesRep, RulesRepDto>().ReverseMap();

            _ = this.CreateMap<PesoArchivo, FileWeight>().ReverseMap();
            _ = this.CreateMap<FileWeight, FileWeightDto>().ReverseMap();

            _ = this.CreateMap<ExtensionesArchivo, FileExtensions>().ReverseMap();
            _ = this.CreateMap<FileExtensions, FileExtensionsDto>().ReverseMap();

            // _ = CreateMap<GeneralPropertiesDto, GeneralProperties>().ReverseMap();

            _ = this.CreateMap<GeneralProperties, GeneralPropertiesDto>()
              .ForMember(dest => dest.Clave, act => act.MapFrom(src => src.Clave))
              .ForMember(dest => dest.CreadoPor, act => act.MapFrom(src => src.Creadopor))
              .ForMember(dest => dest.Descripcion, act => act.MapFrom(src => src.Descripcion))
              .ForMember(dest => dest.FechaCreacion, act => act.MapFrom(src => src.Fechacreacion))
              .ForMember(dest => dest.FechaModificacion, act => act.MapFrom(src => src.Fechamodificacion))
              .ForMember(dest => dest.ModificadoPor, act => act.MapFrom(src => src.Modificadopor))
              .ForMember(dest => dest.H_wye, act => act.MapFrom(src => src.H_wye))
              .ForMember(dest => dest.X_wye, act => act.MapFrom(src => src.X_wye))
              .ForMember(dest => dest.T_wye, act => act.MapFrom(src => src.T_wye))
              .ReverseMap();

            _ = this.CreateMap<CatSidcoInformation, CatSidcoInformationDto>()
                .ForMember(dest => dest.AttributeId, act => act.MapFrom(src => src.AttributeId))
                .ForMember(dest => dest.ClaveSpl, act => act.MapFrom(src => src.ClaveSpl))
                .ForMember(dest => dest.CreadoPor, act => act.MapFrom(src => src.Creadopor))
                .ForMember(dest => dest.FechaCreacion, act => act.MapFrom(src => src.Fechacreacion))
                .ForMember(dest => dest.FechaModificacion, act => act.MapFrom(src => src.Fechamodificacion))
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.ModificadoPor, act => act.MapFrom(src => src.Modificadopor))
                .ReverseMap();

            _ = this.CreateMap<CatSidcoOtherInformation, CatSidcoOtherInformationDto>()
                 .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
                 .ForMember(dest => dest.CreadoPor, act => act.MapFrom(src => src.Creadopor))
                 .ForMember(dest => dest.Dato, act => act.MapFrom(src => src.Dato))
                 .ForMember(dest => dest.Descripcion, act => act.MapFrom(src => src.Descripcion))
                 .ForMember(dest => dest.FechaCreacion, act => act.MapFrom(src => src.Fechacreacion))
                 .ForMember(dest => dest.FechaModificacion, act => act.MapFrom(src => src.Fechamodificacion))
                 .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                 .ForMember(dest => dest.ModificadoPor, act => act.MapFrom(src => src.Modificadopor))
                 .ReverseMap();
        }
    }
}
