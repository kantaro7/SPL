namespace SPL.Reports.Api.AutoMapper
{
    using System;

    using global::AutoMapper;

    using SPL.Domain.SPL.Artifact.ArtifactDesign;
    using SPL.Domain.SPL.Artifact.Nozzles;
    using SPL.Domain.SPL.Artifact.PlateTension;
    using SPL.Domain.SPL.Configuration;
    using SPL.Domain.SPL.Reports;
    using SPL.Domain.SPL.Reports.BPC;
    using SPL.Domain.SPL.Reports.CEM;
    using SPL.Domain.SPL.Reports.CGD;
    using SPL.Domain.SPL.Reports.DPR;
    using SPL.Domain.SPL.Reports.ETD;
    using SPL.Domain.SPL.Reports.FPA;
    using SPL.Domain.SPL.Reports.FPB;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Reports.IND;
    using SPL.Domain.SPL.Reports.ISZ;
    using SPL.Domain.SPL.Reports.NRA;
    using SPL.Domain.SPL.Reports.PCE;
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Domain.SPL.Reports.PEE;
    using SPL.Domain.SPL.Reports.PIM;
    using SPL.Domain.SPL.Reports.PIR;
    using SPL.Domain.SPL.Reports.PLR;
    using SPL.Domain.SPL.Reports.PRD;
    using SPL.Domain.SPL.Reports.RCT;
    using SPL.Domain.SPL.Reports.RDD;
    using SPL.Domain.SPL.Reports.ROD;
    using SPL.Domain.SPL.Reports.RYE;
    using SPL.Domain.SPL.Reports.TAP;
    using SPL.Domain.SPL.Reports.TDP;
    using SPL.Domain.SPL.Reports.TIN;
    using SPL.Domain.SPL.Tests;
    using SPL.Reports.Api.DTOs.Reports;
    using SPL.Reports.Api.DTOs.Reports.ARF;
    using SPL.Reports.Api.DTOs.Reports.BPC;
    using SPL.Reports.Api.DTOs.Reports.CEM;
    using SPL.Reports.Api.DTOs.Reports.CGD;
    using SPL.Reports.Api.DTOs.Reports.DPR;
    using SPL.Reports.Api.DTOs.Reports.ETD;
    using SPL.Reports.Api.DTOs.Reports.FPA;
    using SPL.Reports.Api.DTOs.Reports.FPB;
    using SPL.Reports.Api.DTOs.Reports.FPC;
    using SPL.Reports.Api.DTOs.Reports.IND;
    using SPL.Reports.Api.DTOs.Reports.ISZ;
    using SPL.Reports.Api.DTOs.Reports.Nozzles;
    using SPL.Reports.Api.DTOs.Reports.NRA;
    using SPL.Reports.Api.DTOs.Reports.PCE;
    using SPL.Reports.Api.DTOs.Reports.PCI;
    using SPL.Reports.Api.DTOs.Reports.PEE;
    using SPL.Reports.Api.DTOs.Reports.PIM;
    using SPL.Reports.Api.DTOs.Reports.PIR;
    using SPL.Reports.Api.DTOs.Reports.PRD;
    using SPL.Reports.Api.DTOs.Reports.RDD;
    using SPL.Reports.Api.DTOs.Reports.ROD;
    using SPL.Reports.Api.DTOs.Reports.RYE;
    using SPL.Reports.Api.DTOs.Reports.TAP;
    using SPL.Reports.Api.DTOs.Reports.TDP;
    using SPL.Reports.Api.DTOs.Reports.TIN;
    using SPL.Reports.Infrastructure.Entities;

    public class ReportsProfile : Profile
    {
        public ReportsProfile()
        {
            _ = this.CreateMap<ETDConfigFileReport, ETDConfigFileReportDto>().ReverseMap();
            _ = this.CreateMap<ETDConfigFileReport, SplConfigArchivo>().ReverseMap();

            _ = this.CreateMap<SplReporte, Reports>().ReverseMap();
            _ = this.CreateMap<Reports, ReportsDto>().ReverseMap();

            _ = this.CreateMap<InfoGeneralTypesReports, InfoGeneralTypesReportsDto>().ReverseMap();
            _ = this.CreateMap<InfoGeneralReports, InfoGeneralReportsDto>().ReverseMap();

            _ = this.CreateMap<SplInfoGeneralRad, InfoGeneralReports>()
                .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdCarga))
                .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaCarga))
                .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
                .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
                .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
                .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
                .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
                .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
                .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
                .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
                .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
                .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
                .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
                .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
                .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
                .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
                .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
                .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
                .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();
              _ = this.CreateMap<SplInfoGeneralRan, InfoGeneralReports>()
                .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
                .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
                .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
                .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
                .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
                .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
                .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
                .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
                .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
                .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
                .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
                .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
                .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
                .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
                .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
                .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
                .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
                .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
                .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();

            _ = this.CreateMap<SplInfoGeneralFpc, InfoGeneralReports>()
              .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
              .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
              .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
              .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
              .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
              .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
              .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
              .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
              .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
              .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
              .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
              .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
              .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
              .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
              .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
              .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
              .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
              .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
              .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();

            _ = this.CreateMap<SplInfoGeneralRdt, InfoGeneralReports>()
                .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdCarga))
                .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaCarga))
                .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
                .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
                .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
                .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
                .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
                .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
                .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
                .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
                .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
                .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
                .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
                .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
                .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
                .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
                .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
                .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
                .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();

            _ = this.CreateMap<RADReport, RADReportDto>()
                .ForMember(dest => dest.IdLoad, act => act.MapFrom(src => src.IdLoad))
                .ForMember(dest => dest.LoadDate, act => act.MapFrom(src => src.LoadDate))
                .ForMember(dest => dest.TestNumber, act => act.MapFrom(src => src.TestNumber))
                .ForMember(dest => dest.LanguageKey, act => act.MapFrom(src => src.LanguageKey))
                .ForMember(dest => dest.Customer, act => act.MapFrom(src => src.Customer))
                .ForMember(dest => dest.Capacity, act => act.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.SerialNumber, act => act.MapFrom(src => src.SerialNumber))
                .ForMember(dest => dest.Result, act => act.MapFrom(src => src.Result))
                .ForMember(dest => dest.NameFile, act => act.MapFrom(src => src.NameFile))
                .ForMember(dest => dest.File, act => act.MapFrom(src => Convert.ToBase64String(src.File)))
                .ForMember(dest => dest.TypeReport, act => act.MapFrom(src => src.TypeReport))
                .ForMember(dest => dest.KeyTest, act => act.MapFrom(src => src.KeyTest))
                .ForMember(dest => dest.TypeUnit, act => act.MapFrom(src => src.TypeUnit))
                .ForMember(dest => dest.ThirdWindingType, act => act.MapFrom(src => src.ThirdWindingType))
                .ForMember(dest => dest.Comment, act => act.MapFrom(src => src.Comment))
                .ForMember(dest => dest.DataTests, act => act.MapFrom(src => src.DataTests));

            _ = this.CreateMap<RADReportDto, RADReport>()
                .ForMember(dest => dest.IdLoad, act => act.MapFrom(src => src.IdLoad))
                .ForMember(dest => dest.LoadDate, act => act.MapFrom(src => src.LoadDate))
                .ForMember(dest => dest.TestNumber, act => act.MapFrom(src => src.TestNumber))
                .ForMember(dest => dest.LanguageKey, act => act.MapFrom(src => src.LanguageKey))
                .ForMember(dest => dest.Customer, act => act.MapFrom(src => src.Customer))
                .ForMember(dest => dest.Capacity, act => act.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.SerialNumber, act => act.MapFrom(src => src.SerialNumber))
                .ForMember(dest => dest.Result, act => act.MapFrom(src => src.Result))
                .ForMember(dest => dest.NameFile, act => act.MapFrom(src => src.NameFile))
                .ForMember(dest => dest.File, act => act.MapFrom(src => this.DecodeBase64(src.File)))
                .ForMember(dest => dest.TypeReport, act => act.MapFrom(src => src.TypeReport))
                .ForMember(dest => dest.KeyTest, act => act.MapFrom(src => src.KeyTest))
                .ForMember(dest => dest.TypeUnit, act => act.MapFrom(src => src.TypeUnit))
                .ForMember(dest => dest.ThirdWindingType, act => act.MapFrom(src => src.ThirdWindingType))
                .ForMember(dest => dest.Comment, act => act.MapFrom(src => src.Comment))
                .ForMember(dest => dest.DataTests, act => act.MapFrom(src => src.DataTests));

            _ = this.CreateMap<RDTReport, RDTReportDto>()
                .ForMember(dest => dest.IdLoad, act => act.MapFrom(src => src.IdLoad))
                .ForMember(dest => dest.LoadDate, act => act.MapFrom(src => src.LoadDate))
                .ForMember(dest => dest.TestNumber, act => act.MapFrom(src => src.TestNumber))
                .ForMember(dest => dest.LanguageKey, act => act.MapFrom(src => src.LanguageKey))
                .ForMember(dest => dest.Customer, act => act.MapFrom(src => src.Customer))
                .ForMember(dest => dest.Capacity, act => act.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.SerialNumber, act => act.MapFrom(src => src.SerialNumber))
                .ForMember(dest => dest.Result, act => act.MapFrom(src => src.Result))
                .ForMember(dest => dest.NameFile, act => act.MapFrom(src => src.NameFile))
                .ForMember(dest => dest.File, act => act.MapFrom(src => Convert.ToBase64String(src.File)))
                .ForMember(dest => dest.TypeReport, act => act.MapFrom(src => src.TypeReport))
                .ForMember(dest => dest.KeyTest, act => act.MapFrom(src => src.KeyTest))
                .ForMember(dest => dest.AngularDisplacement, act => act.MapFrom(src => src.AngularDisplacement))
                .ForMember(dest => dest.PostTestBt, act => act.MapFrom(src => src.PostTestBt))
                .ForMember(dest => dest.Ter, act => act.MapFrom(src => src.Ter))
                .ForMember(dest => dest.Connection_sp, act => act.MapFrom(src => src.Connection_sp))
                .ForMember(dest => dest.Pos_at, act => act.MapFrom(src => src.Pos_at))
                .ForMember(dest => dest.Date, act => act.MapFrom(src => src.Date))
                .ForMember(dest => dest.Comment, act => act.MapFrom(src => src.Comment))
                .ForMember(dest => dest.DataTests, act => act.MapFrom(src => src.DataTests));

            _ = this.CreateMap<RDTReportDto, RDTReport>()
                .ForMember(dest => dest.IdLoad, act => act.MapFrom(src => src.IdLoad))
                .ForMember(dest => dest.LoadDate, act => act.MapFrom(src => src.LoadDate))
                .ForMember(dest => dest.TestNumber, act => act.MapFrom(src => src.TestNumber))
                .ForMember(dest => dest.LanguageKey, act => act.MapFrom(src => src.LanguageKey))
                .ForMember(dest => dest.Customer, act => act.MapFrom(src => src.Customer))
                .ForMember(dest => dest.Capacity, act => act.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.SerialNumber, act => act.MapFrom(src => src.SerialNumber))
                .ForMember(dest => dest.Result, act => act.MapFrom(src => src.Result))
                .ForMember(dest => dest.NameFile, act => act.MapFrom(src => src.NameFile))
                .ForMember(dest => dest.File, act => act.MapFrom(src => this.DecodeBase64(src.File)))
                .ForMember(dest => dest.TypeReport, act => act.MapFrom(src => src.TypeReport))
                .ForMember(dest => dest.KeyTest, act => act.MapFrom(src => src.KeyTest))
                .ForMember(dest => dest.AngularDisplacement, act => act.MapFrom(src => src.AngularDisplacement))
                .ForMember(dest => dest.PostTestBt, act => act.MapFrom(src => src.PostTestBt))
                .ForMember(dest => dest.Ter, act => act.MapFrom(src => src.Ter))
                .ForMember(dest => dest.Connection_sp, act => act.MapFrom(src => src.Connection_sp))
                .ForMember(dest => dest.Pos_at, act => act.MapFrom(src => src.Pos_at))
                .ForMember(dest => dest.Date, act => act.MapFrom(src => src.Date))
                .ForMember(dest => dest.Comment, act => act.MapFrom(src => src.Comment))
                .ForMember(dest => dest.DataTests, act => act.MapFrom(src => src.DataTests));

            _ = this.CreateMap<RADTestsDto, RADTests>().ReverseMap();
            _ = this.CreateMap<RDTTestsDto, RDTTests>().ReverseMap();

            _ = this.CreateMap<ColumnDto, Column>().ReverseMap();
            _ = this.CreateMap<HeaderRADTestsDto, HeaderRADTests>().ReverseMap();

            _ = this.CreateMap<SplFiltrosreporte, FilterReports>().ReverseMap();
            _ = this.CreateMap<FilterReports, FilterReportsDto>().ReverseMap();

            _ = this.CreateMap<SplConfiguracionReporte, ConfigurationReports>().ReverseMap();
            _ = this.CreateMap<ConfigurationReports, ConfigurationReportsDto>().ReverseMap();

            _ = this.CreateMap<SplTituloColumnasRad, ColumnTitleRADReports>().ReverseMap();
            _ = this.CreateMap<SplTituloColumnasRdt, ColumnTitleRDTReports>().ReverseMap();
            _ = this.CreateMap<ColumnTitleRADReports, ColumnTitleRADReportsDto>().ReverseMap();
            _ = this.CreateMap<ColumnTitleRDTReports, ColumnTitleRDTReportsDto>().ReverseMap();

            _ = this.CreateMap<TitSeriresParallelReports, SplTitSerieparalelo>().ReverseMap();
            _ = this.CreateMap<CorrectionFactorReports, SplDescFactorcorreccion>().ReverseMap();

            _ = this.CreateMap<TitSeriresParallelReports, TitSeriresParallelReportsDto>().ReverseMap();
            _ = this.CreateMap<CorrectionFactorReports, CorrectionFactorReportsDto>().ReverseMap();

            _ = this.CreateMap<RANReport, RANReportDto>()
                .ForMember(dest => dest.IdLoad, act => act.MapFrom(src => src.IdLoad))
                .ForMember(dest => dest.LoadDate, act => act.MapFrom(src => src.LoadDate))
                .ForMember(dest => dest.TestNumber, act => act.MapFrom(src => src.TestNumber))
                .ForMember(dest => dest.LanguageKey, act => act.MapFrom(src => src.LanguageKey))
                .ForMember(dest => dest.Customer, act => act.MapFrom(src => src.Customer))
                .ForMember(dest => dest.Capacity, act => act.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.SerialNumber, act => act.MapFrom(src => src.SerialNumber))
                .ForMember(dest => dest.Result, act => act.MapFrom(src => src.Result))
                .ForMember(dest => dest.NameFile, act => act.MapFrom(src => src.NameFile))
                .ForMember(dest => dest.File, act => act.MapFrom(src => Convert.ToBase64String(src.File)))
                .ForMember(dest => dest.TypeReport, act => act.MapFrom(src => src.TypeReport))
                .ForMember(dest => dest.KeyTest, act => act.MapFrom(src => src.KeyTest))
                .ForMember(dest => dest.NumberMeasurements, act => act.MapFrom(src => src.NumberMeasurements))
                .ForMember(dest => dest.Rans, act => act.MapFrom(src => src.Rans))
                .ForMember(dest => dest.Comment, act => act.MapFrom(src => src.Comment));

            _ = this.CreateMap<RANReportDto, RANReport>()
                .ForMember(dest => dest.IdLoad, act => act.MapFrom(src => src.IdLoad))
                .ForMember(dest => dest.LoadDate, act => act.MapFrom(src => src.LoadDate))
                .ForMember(dest => dest.TestNumber, act => act.MapFrom(src => src.TestNumber))
                .ForMember(dest => dest.LanguageKey, act => act.MapFrom(src => src.LanguageKey))
                .ForMember(dest => dest.Customer, act => act.MapFrom(src => src.Customer))
                .ForMember(dest => dest.Capacity, act => act.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.SerialNumber, act => act.MapFrom(src => src.SerialNumber))
                .ForMember(dest => dest.Result, act => act.MapFrom(src => src.Result))
                .ForMember(dest => dest.NameFile, act => act.MapFrom(src => src.NameFile))
                .ForMember(dest => dest.File, act => act.MapFrom(src => this.DecodeBase64(src.File)))
                .ForMember(dest => dest.TypeReport, act => act.MapFrom(src => src.TypeReport))
                .ForMember(dest => dest.KeyTest, act => act.MapFrom(src => src.KeyTest))
                .ForMember(dest => dest.NumberMeasurements, act => act.MapFrom(src => src.NumberMeasurements))
                .ForMember(dest => dest.Rans, act => act.MapFrom(src => src.Rans))
                .ForMember(dest => dest.Comment, act => act.MapFrom(src => src.Comment));

        

         

            _ = this.CreateMap<RANReportDetailsDto, RANReportDetails>().ReverseMap();
            _ = this.CreateMap<RANReportDetailsRADto, RANReportDetailsRA>().ReverseMap();
            _ = this.CreateMap<RANReportDetailsTADto, RANReportDetailsTA>().ReverseMap();

            _ = this.CreateMap<FPCTestsDetailsDto, FPCTestsDetails>().ReverseMap();
            _ = this.CreateMap<FPCTestsDto, FPCTests>().ReverseMap();
            _ = this.CreateMap<FPCTestsGeneralDto, FPCTestsGeneral>().ReverseMap();

            _ = this.CreateMap<SplTituloColumnasFpc, ColumnTitleFPCReports>().ReverseMap();

            _ = this.CreateMap<ColumnTitleFPCReportsDto, ColumnTitleFPCReports>().ReverseMap();

            _ = this.CreateMap<RCTTestsDetailsDto, RCTTestsDetails>().ReverseMap();
            _ = this.CreateMap<RCTTestsDto, RCTTests>().ReverseMap();

            _ = this.CreateMap<RCTTestsGeneralDto, RCTTestsGeneral>().ReverseMap();

            _ = this.CreateMap<CorrectionFactorSpecificationDto, CorrectionFactorSpecification>().ReverseMap();

            _ = this.CreateMap<RODTestsDetailsDto, RODTestsDetails>().ReverseMap();
            _ = this.CreateMap<RODTestsDto, RODTests>().ReverseMap();
            _ = this.CreateMap<RODTestsGeneralDto, RODTestsGeneral>().ReverseMap();
            _ = this.CreateMap<ResistDesignDto, ResistDesign>().ReverseMap();

            _ = this.CreateMap<ReportPDF, ReportPDFDto>()
              .ForMember(dest => dest.IdLoad, act => act.MapFrom(src => src.IdLoad))
              .ForMember(dest => dest.File, act => act.MapFrom(src => Convert.ToBase64String(src.File))).ReverseMap();

            _ = this.CreateMap<FPBTestsDetailsDto, FPBTestsDetails>().ReverseMap();
            _ = this.CreateMap<FPBTestsDto, FPBTests>().ReverseMap();
            _ = this.CreateMap<FPBTestsGeneralDto, FPBTestsGeneral>().ReverseMap();

            _ = this.CreateMap<RecordNozzleInformationDto, RecordNozzleInformation>().ReverseMap();
            _ = this.CreateMap<CorrectionFactorsXMarksXTypesDto, CorrectionFactorsXMarksXTypes>().ReverseMap();

            _ = this.CreateMap<SplTituloColumnasFpc, ColumnTitleFPCReports>().ReverseMap();

            _ = this.CreateMap<ColumnTitleFPCReportsDto, ColumnTitleFPCReports>().ReverseMap();

            _ = this.CreateMap<SplInfoGeneralRod, InfoGeneralReports>()
              .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
              .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
              .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
              .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
              .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
              .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
              .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
              .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
              .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
              .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
              .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
              .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
              .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
              .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
              .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
              .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
              .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
              .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
              .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();

            _ = this.CreateMap<SplInfoGeneralRct, InfoGeneralReports>()
              .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
              .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
              .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
              .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
              .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
              .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
              .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
              .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
              .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
              .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
              .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
              .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
              .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
              .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
              .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
              .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
              .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
              .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
              .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();

            _ = this.CreateMap<SplInfoGeneralFpb, InfoGeneralReports>()
              .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
              .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
              .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
              .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
              .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
              .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
              .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
              .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
              .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
              .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
              .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
              .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
              .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
              .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
              .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
              .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
              .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
              .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
              .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();

            _ = this.CreateMap<PCETestsDetailsDto, PCETestsDetails>().ReverseMap();
            _ = this.CreateMap<PCETestsDto, PCETests>().ReverseMap();
            _ = this.CreateMap<PCETestsGeneralDto, PCETestsGeneral>().ReverseMap();

            _ = this.CreateMap<WarrantiesArtifactDto, WarrantiesArtifact>().ReverseMap();
            _ = this.CreateMap<RODTestsGeneralDto, RODTestsGeneral>().ReverseMap();
            _ = this.CreateMap<PCETestsGeneralDto, PCETestsGeneral>().ReverseMap();
            _ = this.CreateMap<PlateTensionDto, PlateTension>().ReverseMap();

            _ = this.CreateMap<PCITestsGeneralDto, PCITestGeneral>().ReverseMap();
            //_ = this.CreateMap<PCITestsDto, PCITest>().ReverseMap();  
            //_ = this.CreateMap<PCITestsDetailsDto, PCITestDetail>().ReverseMap();
            //_ = this.CreateMap<PCIOutTestsDto, PCIOutTests>().ReverseMap();

            _ = this.CreateMap<PLRTestsDetailsDto, PLRTestsDetails>().ReverseMap();
            _ = this.CreateMap<PLRTestsDto, PLRTests>().ReverseMap();
            _ = this.CreateMap<PLRTestsGeneralDto, PLRTestsGeneral>().ReverseMap();

            _ = this.CreateMap<PRDTestsDetailsDto, PRDTestsDetails>().ReverseMap();
            _ = this.CreateMap<PRDTestsDto, PRDTests>().ReverseMap();
            _ = this.CreateMap<PRDTestsGeneralDto, PRDTestsGeneral>().ReverseMap();

            _ = this.CreateMap<PEETestsDetailsDto, PEETestsDetails>().ReverseMap();
            _ = this.CreateMap<PEETestsDto, PEETests>().ReverseMap();
            _ = this.CreateMap<PEETestsGeneralDto, PEETestsGeneral>().ReverseMap();


            _ = this.CreateMap<SplInfoGeneralRod, InfoGeneralReports>()
            .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
            .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
            .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
            .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
            .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
            .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
            .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
            .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
            .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
            .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
            .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
            .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
            .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
            .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
            .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
            .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
            .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
            .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
            .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();



            _ = this.CreateMap<SplInfoGeneralPci, InfoGeneralReports>()
               .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
               .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
               .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
               .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
               .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
               .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
               .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
               .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
               .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
               .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
               .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
               .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
               .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
               .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
               .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
               .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
               .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
               .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
               .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();



            _ = this.CreateMap<SplInfoGeneralPce, InfoGeneralReports>()
          .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
          .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
          .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
          .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
          .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
          .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
          .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
          .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
          .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
          .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
          .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
          .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
          .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
          .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
          .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
          .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
          .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
          .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
          .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();

            _ = this.CreateMap<SplInfoGeneralPrd, InfoGeneralReports>()
   .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
   .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
   .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
   .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
   .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
   .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
   .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
   .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
   .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
   .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
   .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
   .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
   .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
   .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
   .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
   .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
   .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
   .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
   .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();

            _ = this.CreateMap<SplInfoGeneralPlr, InfoGeneralReports>()
 .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
 .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
 .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
 .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
 .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
 .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
 .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
 .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
 .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
 .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
 .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
 .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
 .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
 .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
 .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
 .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
 .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
 .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
 .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();


 _ = this.CreateMap<SplInfoGeneralPee, InfoGeneralReports>()
 .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
 .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
 .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
 .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
 .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
 .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
 .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
 .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
 .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
 .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
 .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
 .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
 .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
 .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
 .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
 .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
 .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
 .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
 .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();


            _ = this.CreateMap<ISZTestsDetailsDto, ISZTestsDetails>().ReverseMap();
            _ = this.CreateMap<OutISZTestsDto, OutISZTests>().ReverseMap();
            _ = this.CreateMap<ISZTestsGeneralDto, ISZTestsGeneral>().ReverseMap();
            _ = this.CreateMap<SeccionesISZTestDetailsDto, SeccionesISZTestDetails>().ReverseMap();


            _ = this.CreateMap<RYETestsDetailsDto, RYETestsDetails>().ReverseMap();
            _ = this.CreateMap<OutRYETestsDto, OutRYETests>().ReverseMap();
            _ = this.CreateMap<RYETestsGeneralDto, RYETestsGeneral>().ReverseMap();



            _ = this.CreateMap<PIMTestsDetailsDto, PIMTestsDetails>().ReverseMap();
            _ = this.CreateMap<PIMTestsGeneralDto, PIMTestsGeneral>().ReverseMap();


            _ = this.CreateMap<PIRTestsDetailsDto, PIRTestsDetails>().ReverseMap();
            _ = this.CreateMap<PIRTestsGeneralDto, PIRTestsGeneral>().ReverseMap();


            _ = this.CreateMap<FilesDto, Files>().ReverseMap();


            _ = this.CreateMap<TINTestsGeneralDto, TINTestsGeneral>().ReverseMap();

            _ = this.CreateMap<TAPTestsDetailsDto, TAPTestsDetails>().ReverseMap();
            _ = this.CreateMap<TAPTestsDto, TAPTests>().ReverseMap();
            _ = this.CreateMap<TAPTestsGeneralDto, TAPTestsGeneral>().ReverseMap();



            _ = this.CreateMap<SplTituloColumnasCem, ColumnTitleCEMReports>().ReverseMap();
            _ = this.CreateMap<ColumnTitleCEMReports, ColumnTitleCEMReportsDto>().ReverseMap();


            _ = this.CreateMap<CEMTestsDetailsDto, CEMTestsDetails>().ReverseMap();
            _ = this.CreateMap<CEMTestsGeneralDto, CEMTestsGeneral>().ReverseMap();



            _ = this.CreateMap<SplInfoGeneralIsz, InfoGeneralReports>()
             .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
             .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
             .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
             .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
             .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
             .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
             .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
             .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
             .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
             .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
             .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
             .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
             .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
             .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
             .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
             .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
             .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
             .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
             .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();



            _ = this.CreateMap<SplInfoGeneralRye, InfoGeneralReports>()
            .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
            .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
            .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
            .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
            .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
            .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
            .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
            .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
            .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
            .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
            .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
            .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
            .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
            .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
            .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
            .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
            .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
            .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
            .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();


            _ = this.CreateMap<SplInfoGeneralPim, InfoGeneralReports>()
         .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
         .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
         .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
         .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
         .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
         .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
         .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
         .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
         .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
         .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
         .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
         .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
         .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
         .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
         .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
         .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
         .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
         .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
         .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();



            _ = this.CreateMap<SplInfoGeneralPir, InfoGeneralReports>()
       .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
       .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
       .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
       .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
       .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
       .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
       .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
       .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
       .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
       .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
       .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
       .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
       .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
       .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
       .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
       .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
       .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
       .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
       .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();



            _ = this.CreateMap<SplInfoGeneralTin, InfoGeneralReports>()
       .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
       .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
       .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
       .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
       .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
       .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
       .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
       .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
       .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
       .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
       .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
       .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
       .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
       .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
       .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
       .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
       .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
       .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
       .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();

                _ = this.CreateMap<SplInfoGeneralTap, InfoGeneralReports>()
        .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
        .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
        .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
        .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
        .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
        .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
        .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
        .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
        .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
        .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
        .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
        .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
        .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
        .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
        .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
        .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
        .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
        .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
    .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();

                    _ = this.CreateMap<SplInfoGeneralTdp, InfoGeneralReports>()
        .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
        .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
        .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
        .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
        .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
        .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
        .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
        .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
        .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
        .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
        .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
        .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
        .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
        .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
        .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
        .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
        .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
        .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
        .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();

            _ = this.CreateMap<SplInfoGeneralCem, InfoGeneralReports>()
        .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
        .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
        .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
        .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
        .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
        .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
        .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
        .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
        .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
        .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
        .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
        .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
        .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
        .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
        .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
        .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
        .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
        .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
        .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();









            _ = this.CreateMap<SplInfoGeneralCgd, InfoGeneralReports>()
    .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
    .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
    .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
    .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
    .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
    .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
    .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
    .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
    .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
    .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
    .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
    .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
    .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
    .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
    .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
    .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
    .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
    .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
    .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();

            _ = this.CreateMap<SplInfoGeneralRdd, InfoGeneralReports>()
   .ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
   .ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
   .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
   .ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
   .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
   .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
   .ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
   .ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
   .ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
   .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
   .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
   .ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
   .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
   .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
   .ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
   .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
   .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
   .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
   .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();

            _ = this.CreateMap<SplInfoGeneralArf, InfoGeneralReports>()
.ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
.ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
.ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
.ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
.ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
.ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
.ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
.ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
.ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
.ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
.ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
.ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
.ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
.ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
.ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
.ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
.ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
.ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
.ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();



            _ = this.CreateMap<SplInfoGeneralInd, InfoGeneralReports>()
.ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
.ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
.ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
.ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
.ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
.ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
.ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
.ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
.ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
.ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
.ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
.ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
.ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
.ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
.ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
.ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
.ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
.ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
.ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();

            _ = this.CreateMap<SplInfoGeneralFpa, InfoGeneralReports>()
.ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
.ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
.ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
.ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
.ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
.ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
.ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
.ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
.ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
.ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
.ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
.ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
.ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
.ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
.ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
.ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
.ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
.ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
.ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();


            _ = this.CreateMap<SplInfoGeneralBpc, InfoGeneralReports>()
.ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
.ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
.ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
.ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
.ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
.ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
.ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
.ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
.ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
.ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
.ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
.ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
.ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
.ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
.ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
.ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
.ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
.ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
.ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();


            _ = this.CreateMap<SplInfoGeneralNra, InfoGeneralReports>()
.ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
.ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
.ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
.ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
.ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
.ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
.ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
.ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
.ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
.ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
.ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
.ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
.ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
.ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
.ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
.ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
.ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
.ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
.ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();


            _ = this.CreateMap<SplInfoGeneralEtd, InfoGeneralReports>()
.ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
.ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
.ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
.ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
.ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
.ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
.ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
.ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
.ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
.ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
.ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
.ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
.ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
.ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
.ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
.ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
.ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
.ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
.ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();

            _ = this.CreateMap<SplInfoGeneralDpr, InfoGeneralReports>()
.ForMember(dest => dest.IdCarga, act => act.MapFrom(src => src.IdRep))
.ForMember(dest => dest.FechaCarga, act => act.MapFrom(src => src.FechaRep))
.ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.NoSerie))
.ForMember(dest => dest.NoPrueba, act => act.MapFrom(src => src.NoPrueba))
.ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
.ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
.ForMember(dest => dest.Cliente, act => act.MapFrom(src => src.Cliente))
.ForMember(dest => dest.Capacidad, act => act.MapFrom(src => src.Capacidad))
.ForMember(dest => dest.Resultado, act => act.MapFrom(src => src.Resultado))
.ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
.ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
.ForMember(dest => dest.Archivo, act => act.MapFrom(src => Convert.ToBase64String(src.Archivo)))
.ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
.ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
.ForMember(dest => dest.Comentario, act => act.MapFrom(src => src.Comentario))
.ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
.ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
.ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
.ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion)).ReverseMap();


            _ = this.CreateMap<TDPCalibrationMeasurementDto, TDPCalibrationMeasurement>().ReverseMap();
            _ = this.CreateMap<TDPTerminalsDto, TDPTerminals>().ReverseMap();
            _ = this.CreateMap<TDPTestsDetailsDto, TDPTestsDetails>().ReverseMap();
            _ = this.CreateMap<TDPTestsDto, TDPTests>().ReverseMap();
            _ = this.CreateMap<TDPTestsGeneralDto, TDPTestsGeneral>().ReverseMap();

            _ = this.CreateMap<CGDTestsDetails, CGDTestsDetailsDto>().ReverseMap();
            _ = this.CreateMap<CGDTests, CGDTestsDto>().ReverseMap();
            _ = this.CreateMap<CGDTestsGeneral, CGDTestsGeneralDto>().ReverseMap();

            _ = this.CreateMap<ARFTests, ARFTestsDto>().ReverseMap();
            _ = this.CreateMap<ARFTestsGeneral, ARFTestsGeneralDto>().ReverseMap();



   
            _ = this.CreateMap<OutRDDTestsDto, OutRDDTests>().ReverseMap();
            _ = this.CreateMap<RDDTestsDetailsDto, RDDTestsDetails>().ReverseMap();
            _ = this.CreateMap<RDDTestsGeneralDto, RDDTestsGeneral>().ReverseMap();


            _ = this.CreateMap<INDTestsDetailsDto, INDTestsDetails>().ReverseMap();
            _ = this.CreateMap<INDTestsGeneralDto, INDTestsGeneral>().ReverseMap();


            _ = this.CreateMap<FPADielectricStrengthDto, FPADielectricStrength>().ReverseMap();
            _ = this.CreateMap<FPAGasContentDto, FPAGasContent>().ReverseMap();
            _ = this.CreateMap<FPAPowerFactorDto, FPAPowerFactor>().ReverseMap();
            _ = this.CreateMap<FPATestsDetailsDto, FPATestsDetails>().ReverseMap();
            _ = this.CreateMap<FPATestsDto, FPATests>().ReverseMap();
            _ = this.CreateMap<FPATestsGeneralDto, FPATestsGeneral>().ReverseMap();
            _ = this.CreateMap<FPAWaterContentDto, FPAWaterContent>().ReverseMap();

            _ = this.CreateMap<BPCTestsGeneralDto, BPCTestsGeneral>().ReverseMap();



            _ = this.CreateMap<MatrixNRATestsDto, MatrixNRATests>().ReverseMap();
            _ = this.CreateMap<NRATestsDetailsOctDto, NRATestsDetailsOct>().ReverseMap();
            _ = this.CreateMap<NRATestsDetailsRuiDto, NRATestsDetailsRui>().ReverseMap();
            _ = this.CreateMap<NRATestsDto, NRATests>().ReverseMap();
            _ = this.CreateMap<NRATestsGeneralDto, NRATestsGeneral>().ReverseMap();



    
            _ = this.CreateMap<DPRTerminalsDto, DPRTerminals>().ReverseMap();
            _ = this.CreateMap<DPRTestsDetailsDto, DPRTestsDetails>().ReverseMap();
            _ = this.CreateMap<DPRTestsDto, DPRTests>().ReverseMap();
            _ = this.CreateMap<DPRTestsGeneralDto, DPRTestsGeneral>().ReverseMap();
            _ = this.CreateMap<ConsolidatedReportDto, ConsolidatedReport>().ReverseMap();
            _ = this.CreateMap<TypeConsolidatedReportDto, TypeConsolidatedReport>().ReverseMap();
            _ = this.CreateMap<CheckInfoRODDto, CheckInfoROD>().ReverseMap();


            _ = this.CreateMap<ETDTestsDetailsDto, ETDTestsDetails>().ReverseMap();
            _ = this.CreateMap<ETDTestsDto, ETDTests>().ReverseMap();
            _ = this.CreateMap<ETDReportDto, ETDReport>().ReverseMap();
            _ = this.CreateMap<ETDTestsGeneralDto, ETDTestsGeneral>().ReverseMap();
            _ = this.CreateMap<GraphicETDTestsDto, GraphicETDTests>().ReverseMap();
            _ = this.CreateMap<GraphicETDTestsDto, GraphicETDTests>().ReverseMap();

        }
        private byte[] DecodeBase64(string pBase64)
        {
            string result = pBase64[(pBase64.IndexOf(",") + 1)..];
            return Convert.FromBase64String(result);
        }
    }
}
