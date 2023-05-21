namespace SPL.Configuration.Api.AutoMapper
{
    using AutoMapper;

    using global::AutoMapper;

    using SPL.Configuration.Api.DTOs.Configuration;
    using SPL.Configuration.Infrastructure.Entities;
    using SPL.Domain.SPL.Configuration;
    

    public class ConfigurationProfile : Profile
    {
        public ConfigurationProfile()
        {
           
            _ = this.CreateMap<SplFactorcorFpc, CorrectionFactorSpecification>().ReverseMap();
            _ = this.CreateMap<CorrectionFactorSpecification, CorrectionFactorSpecificationDto>().ReverseMap();

            _ = this.CreateMap<SplMarcasBoq, NozzleMarks>().ReverseMap();
            _ = this.CreateMap<SplTiposxmarcaBoq, TypesNozzleMarks>().ReverseMap();
            _ = this.CreateMap<SplFactorcorFpb, CorrectionFactorsXMarksXTypes>().ReverseMap();

            _ = this.CreateMap<NozzleMarksDto, NozzleMarks>().ReverseMap();
            _ = this.CreateMap<TypesNozzleMarksDto, TypesNozzleMarks>().ReverseMap();
            _ = this.CreateMap<CorrectionFactorsXMarksXTypesDto, CorrectionFactorsXMarksXTypes>().ReverseMap();

            _ = this.CreateMap<SplDescFactorcor, CorrectionFactorsDesc>().ReverseMap();
            _ = this.CreateMap<CorrectionFactorsDescDto, CorrectionFactorsDesc>().ReverseMap();


            _ = this.CreateMap<SplValidationTestsIsz, ValidationTestsIsz>().ReverseMap();
            _ = this.CreateMap<ValidationTestsIsz, ValidationTestsIszDto>().ReverseMap();


            _ = this.CreateMap<SplContgasCgd, ContGasCGD>().ReverseMap();
            _ = this.CreateMap<ContGasCGD, ContGasCGDDto>().ReverseMap();



            _ = this.CreateMap<SplInfoOctava, InformationOctaves>().ReverseMap();
            _ = this.CreateMap<InformationOctaves, InformationOctavesDto>().ReverseMap();



            _ = this.CreateMap<SplInfoaparatoEst, StabilizationDesignData>().ReverseMap();
            _ = this.CreateMap<StabilizationDesignData, StabilizationDesignDataDto>().ReverseMap();



            _ = this.CreateMap<SplFactorcorEtd, CorrectionFactorkWTypeCooling>().ReverseMap();
            _ = this.CreateMap<CorrectionFactorkWTypeCooling, CorrectionFactorkWTypeCoolingDto>().ReverseMap();


            _ = this.CreateMap<SplDatosgralEst, StabilizationData>().ReverseMap();
            _ = this.CreateMap<StabilizationData, StabilizationDataDto>().ReverseMap();
            _ = this.CreateMap<SplDatosgralEst, StabilizationDetailsData>().ReverseMap();
            _ = this.CreateMap<StabilizationDetailsData, StabilizationDetailsDataDto>().ReverseMap();



            _ = this.CreateMap<SplInfoLaboratorio, InformationLaboratories>().ReverseMap();
            _ = this.CreateMap<InformationLaboratories, InformationLaboratoriesDto>().ReverseMap();


            _ = this.CreateMap<DetailCuttingDataDto, DetailCuttingData>().ReverseMap();
            _ = this.CreateMap<HeaderCuttingDataDto, HeaderCuttingData>().ReverseMap();
            _ = this.CreateMap<SectionCuttingDataDto, SectionCuttingData>().ReverseMap();


            _ = this.CreateMap<SplCortegralEst, HeaderCuttingData>().ReverseMap();
            _ = this.CreateMap<SplCorteseccEst, SectionCuttingData>().ReverseMap();
            _ = this.CreateMap<SplCortedetaEst, DetailCuttingData>().ReverseMap();


            _ = this.CreateMap<SplDatosdetEst, StabilizationDetailsData>().ReverseMap();




      
        }
    }
}


