namespace SPL.Tests.Api.AutoMapper
{
    using global::AutoMapper;

    using SPL.Tests.Api.DTOs.Tests.FPB;

    using SPL.Domain.SPL.Artifact.ArtifactDesign;
    using SPL.Domain.SPL.Artifact.Nozzles;
    using SPL.Domain.SPL.Configuration;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.FPB;
    using SPL.Domain.SPL.Tests.FPC;
    using SPL.Domain.SPL.Tests.ROD;
    using SPL.Domain.SPL.Tests.RCT;
    using SPL.Tests.Api.DTOs.Tests;

    using SPL.Tests.Api.DTOs.Tests.FPC;
    using SPL.Tests.Api.DTOs.Tests.Nozzles;
    using SPL.Tests.Api.DTOs.Tests.ROD;
    using SPL.Tests.Api.DTOs.Tests.RCT;
    using SPL.Tests.Infrastructure.Entities;
    using SPL.Tests.Api.DTOs.Tests.PCI;
    using SPL.Domain.SPL.Tests.PCI;
    using SPL.Tests.Api.DTOs.Tests.PCE;

    using SPL.Domain.SPL.Tests.PCE;
    using SPL.Tests.Api.DTOs.Tests.PLR;
    using SPL.Domain.SPL.Tests.PLR;
    using SPL.Domain.SPL.Tests.PRD;
    using SPL.Tests.Api.DTOs.Tests.PRD;
    using SPL.Domain.SPL.Artifact.PlateTension;
    using SPL.Tests.Api.DTOs.Tests.PEE;
    using SPL.Domain.SPL.Tests.PEE;
    using SPL.Tests.Api.DTOs.Tests.ISZ;
    using SPL.Domain.SPL.Tests.ISZ;
    using SPL.Tests.Api.DTOs.Tests.Artifactdesign;
    using SPL.Tests.Api.DTOs.Tests.RYE;
    using SPL.Tests.Api.DTOs.Tests.TAP;
    using SPL.Domain.SPL.Tests.TAP;
    using SPL.Tests.Api.DTOs.Tests.TDP;
    using SPL.Domain.SPL.Tests.TDP;
    using SPL.Tests.Api.DTOs.Tests.CGD;
    using SPL.Domain.SPL.Tests.CGD;
    using SPL.Tests.Api.DTOs.Tests.RDD;

    using SPL.Domain.SPL.Tests.RDD;
    using SPL.Tests.Api.DTOs.Tests.FPA;
    using SPL.Domain.SPL.Tests.FPA;
    using SPL.Tests.Api.DTOs.Tests.NRA;
    using SPL.Domain.SPL.Tests.NRA;
    using StabilizationData = Domain.SPL.Tests.StabilizationData;
    using StabilizationDetailsData = Domain.SPL.Tests.StabilizationDetailsData;
    using SPL.Tests.Api.DTOs.Tests.DPR;
    using SPL.Domain.SPL.Tests.DPR;
    using SPL.Tests.Api.DTOs.Tests.ETD;
    using SPL.Domain.SPL.Tests.ETD;
    using SPL.Tests.Api.DTOs.Tests.CuttingData;
    using static SPL.Domain.SPL.Tests.ISZ.OutISZTests;

    public class TestsProfile : Profile
    {
        public TestsProfile()
        {



            _ = CreateMap<InformationArtifactDto, InformationArtifact>().ReverseMap();

            // _ = CreateMap<GeneralArtifactDto, GeneralArtifact>().ReverseMap();
            //_ = CreateMap<NozzlesArtifactDto, NozzlesArtifact>().ReverseMap();
            // _ = CreateMap<CharacteristicsArtifactDto, CharacteristicsArtifact>().ReverseMap();
            //_ = CreateMap<WarrantiesArtifactDto, WarrantiesArtifact>().ReverseMap();
            // _ = CreateMap<LightningRodArtifactDto, LightningRodArtifact>().ReverseMap();
            // _ = CreateMap<ChangingTablesArtifactDto, ChangingTablesArtifact>().ReverseMap();
            // _ = CreateMap<LabTestsArtifactDto, LabTestsArtifact>().ReverseMap();
            // _ = CreateMap<RulesArtifactDto, RulesArtifact>().ReverseMap();

            _ = CreateMap<InformationArtifactDto, InformationArtifact>().ReverseMap();
            //_ = CreateMap<TapBaanDto, TapBaan>().ReverseMap();
            // _ = CreateMap<NBAINeutroDto, NBAINeutro>().ReverseMap();
            //  _ = CreateMap<ConnectionTypesDto, ConnectionTypes>().ReverseMap();
            // _ = CreateMap<DerivationsDto, Derivations>().ReverseMap();
            // _ = CreateMap<NBAIBilKvDto, NBAIBilKv>().ReverseMap();
            // _ = CreateMap<TapsDto, Taps>().ReverseMap();
            // _ = CreateMap<VoltageKVDto, VoltageKV>().ReverseMap();

            _ = CreateMap<AllChangingTablesArtifactDto, AllChangingTablesArtifact>().ReverseMap();
            _ = CreateMap<AllCharacteristicsArtifactDto, AllCharacteristicsArtifact>().ReverseMap();

            CreateMap<GeneralArtifact, GeneralArtifactDto>()
                  .ForMember(dest => dest.Applicationid, act => act.MapFrom(src => src.Applicationid))
                  .ForMember(dest => dest.AltitudeF1, act => act.MapFrom(src => src.AltitudeF1))
                  .ForMember(dest => dest.AltitudeF2, act => act.MapFrom(src => src.AltitudeF2))
                  .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
                  .ForMember(dest => dest.CreadoPor, act => act.MapFrom(src => src.Creadopor))
                  .ForMember(dest => dest.CustomerName, act => act.MapFrom(src => src.CustomerName))
                  .ForMember(dest => dest.Descripcion, act => act.MapFrom(src => src.Descripcion))
                  .ForMember(dest => dest.DesplazamientoAngular, act => act.MapFrom(src => src.DesplazamientoAngular))
                  .ForMember(dest => dest.FechaCreacion, act => act.MapFrom(src => src.Fechacreacion))
                  .ForMember(dest => dest.FechaModificacion, act => act.MapFrom(src => src.Fechamodificacion))
                  .ForMember(dest => dest.LanguageId, act => act.MapFrom(src => src.LanguageId))
                  .ForMember(dest => dest.Norma, act => act.MapFrom(src => src.Norma))
                  .ForMember(dest => dest.Phases, act => act.MapFrom(src => src.Phases))
                  .ForMember(dest => dest.PolarityId, act => act.MapFrom(src => src.PolarityId))
                  .ForMember(dest => dest.PolarityOther, act => act.MapFrom(src => src.PolarityOther))
                  .ForMember(dest => dest.PoNumeric, act => act.MapFrom(src => src.PoNumeric))
                  .ForMember(dest => dest.StandardId, act => act.MapFrom(src => src.Standardid))
                  .ForMember(dest => dest.TipoUnidad, act => act.MapFrom(src => src.TipoUnidad))
                  .ForMember(dest => dest.TypeTrafoId, act => act.MapFrom(src => src.Typetrafoid))
                  .ForMember(dest => dest.Frecuency, act => act.MapFrom(src => src.Frecuency))
                  .ForMember(dest => dest.ModificadoPor, act => act.MapFrom(src => src.Modificadopor))
                  .ForMember(dest => dest.OilType, act => act.MapFrom(src => src.TipoAceite))
                  .ForMember(dest => dest.OilBrand, act => act.MapFrom(src => src.MarcaAceite))
                  .ForMember(dest => dest.OrderCode, act => act.MapFrom(src => src.OrderCode)).ReverseMap();

            CreateMap<NozzlesArtifact, NozzlesArtifactDto>()
                 .ForMember(dest => dest.BilClass, act => act.MapFrom(src => src.BilClass))
                 .ForMember(dest => dest.BilClassOther, act => act.MapFrom(src => src.BilClassOther))
                 .ForMember(dest => dest.ColumnTitle, act => act.MapFrom(src => src.ColumnTitle))
                 .ForMember(dest => dest.ColumnTypeId, act => act.MapFrom(src => src.ColumnTypeId))
                 .ForMember(dest => dest.CorrienteUnidad, act => act.MapFrom(src => src.CorrienteUnidad))
                 .ForMember(dest => dest.CurrentAmps, act => act.MapFrom(src => src.CurrentAmps))
                 .ForMember(dest => dest.CreadoPor, act => act.MapFrom(src => src.Creadopor))
                 .ForMember(dest => dest.CurrentAmpsReq, act => act.MapFrom(src => src.CurrentAmpsReq))
                 .ForMember(dest => dest.FechaCreacion, act => act.MapFrom(src => src.Fechacreacion))
                 .ForMember(dest => dest.FechaModificacion, act => act.MapFrom(src => src.Fechamodificacion))
                 .ForMember(dest => dest.ModificadoPor, act => act.MapFrom(src => src.Modificadopor))
                 .ForMember(dest => dest.OrderCode, act => act.MapFrom(src => src.OrderCode))
                 .ForMember(dest => dest.OrderIndex, act => act.MapFrom(src => src.OrderIndex))
                 .ForMember(dest => dest.Qty, act => act.MapFrom(src => src.Qty))
                 .ForMember(dest => dest.VoltageClass, act => act.MapFrom(src => src.VoltageClass))
                 .ForMember(dest => dest.VoltageClassOther, act => act.MapFrom(src => src.VoltageClassOther))

             .ReverseMap();

            CreateMap<CharacteristicsArtifact, CharacteristicsArtifactDto>()
                .ForMember(dest => dest.CoolingType, act => act.MapFrom(src => src.CoolingType))
                .ForMember(dest => dest.CreadoPor, act => act.MapFrom(src => src.Creadopor))
                .ForMember(dest => dest.DevAwr, act => act.MapFrom(src => src.DevAwr))
                .ForMember(dest => dest.FechaCreacion, act => act.MapFrom(src => src.Fechacreacion))
                .ForMember(dest => dest.FechaModificacion, act => act.MapFrom(src => src.Fechamodificacion))
                .ForMember(dest => dest.Hstr, act => act.MapFrom(src => src.Hstr))
                .ForMember(dest => dest.ModificadoPor, act => act.MapFrom(src => src.Modificadopor))
                .ForMember(dest => dest.Mvaf1, act => act.MapFrom(src => src.Mvaf1))
                .ForMember(dest => dest.Mvaf2, act => act.MapFrom(src => src.Mvaf2))
                .ForMember(dest => dest.Mvaf3, act => act.MapFrom(src => src.Mvaf3))
                .ForMember(dest => dest.Mvaf4, act => act.MapFrom(src => src.Mvaf4))
                .ForMember(dest => dest.OrderCode, act => act.MapFrom(src => src.OrderCode))
                .ForMember(dest => dest.OverElevation, act => act.MapFrom(src => src.OverElevation))
                .ForMember(dest => dest.Secuencia, act => act.MapFrom(src => src.Secuencia)).ReverseMap();

            CreateMap<WarrantiesArtifact, WarrantiesArtifactDto>()
                .ForMember(dest => dest.CreadoPor, act => act.MapFrom(src => src.Creadopor))
                .ForMember(dest => dest.FechaCreacion, act => act.MapFrom(src => src.Fechacreacion))
                .ForMember(dest => dest.FechaModificacion, act => act.MapFrom(src => src.Fechamodificacion))
                .ForMember(dest => dest.Iexc100, act => act.MapFrom(src => src.Iexc100))
                .ForMember(dest => dest.Iexc110, act => act.MapFrom(src => src.Iexc110))
                .ForMember(dest => dest.Kwaux1, act => act.MapFrom(src => src.Kwaux1))
                .ForMember(dest => dest.Kwaux2, act => act.MapFrom(src => src.Kwaux2))
                .ForMember(dest => dest.Kwaux3, act => act.MapFrom(src => src.Kwaux3))
                .ForMember(dest => dest.Kwaux4, act => act.MapFrom(src => src.Kwaux4))
                .ForMember(dest => dest.Kwcu, act => act.MapFrom(src => src.Kwcu))
                .ForMember(dest => dest.KwcuKv, act => act.MapFrom(src => src.KwcuKv))
                .ForMember(dest => dest.KwcuMva, act => act.MapFrom(src => src.KwcuMva))
                .ForMember(dest => dest.Kwfe100, act => act.MapFrom(src => src.Kwfe100))
                .ForMember(dest => dest.Kwfe110, act => act.MapFrom(src => src.Kwfe110))
                .ForMember(dest => dest.Kwtot100, act => act.MapFrom(src => src.Kwtot100))
                .ForMember(dest => dest.Kwtot110, act => act.MapFrom(src => src.Kwtot110))
                .ForMember(dest => dest.ModificadoPor, act => act.MapFrom(src => src.Modificadopor))
                .ForMember(dest => dest.NoiseFa1, act => act.MapFrom(src => src.NoiseFa1))
                .ForMember(dest => dest.NoiseFa2, act => act.MapFrom(src => src.NoiseFa2))
                .ForMember(dest => dest.NoiseOa, act => act.MapFrom(src => src.NoiseOa))
                .ForMember(dest => dest.OrderCode, act => act.MapFrom(src => src.OrderCode))
                .ForMember(dest => dest.TolerancyKwAux, act => act.MapFrom(src => src.TolerancyKwAux))
                .ForMember(dest => dest.TolerancyKwCu, act => act.MapFrom(src => src.TolerancyKwCu))
                .ForMember(dest => dest.TolerancyKwfe, act => act.MapFrom(src => src.TolerancyKwfe))
                .ForMember(dest => dest.TolerancyKwtot, act => act.MapFrom(src => src.TolerancyKwtot))
                .ForMember(dest => dest.TolerancyZpositive, act => act.MapFrom(src => src.TolerancyZpositive))
                .ForMember(dest => dest.TolerancyZpositive2, act => act.MapFrom(src => src.TolerancyZpositive2))
                .ForMember(dest => dest.ZPositiveHx, act => act.MapFrom(src => src.ZPositiveHx))
                .ForMember(dest => dest.ZPositiveHy, act => act.MapFrom(src => src.ZPositiveHy))
                .ForMember(dest => dest.ZPositiveHy, act => act.MapFrom(src => src.ZPositiveHy))
                .ForMember(dest => dest.ZPositiveXy, act => act.MapFrom(src => src.ZPositiveXy)).ReverseMap();

            CreateMap<LightningRodArtifact, LightningRodArtifactDto>()
                .ForMember(dest => dest.ColumnTitle, act => act.MapFrom(src => src.ColumnTitle))
                .ForMember(dest => dest.ColumnTypeId, act => act.MapFrom(src => src.ColumnTypeId))
                .ForMember(dest => dest.CreadoPor, act => act.MapFrom(src => src.Creadopor))
                .ForMember(dest => dest.FechaCreacion, act => act.MapFrom(src => src.Fechacreacion))
                .ForMember(dest => dest.FechaModificacion, act => act.MapFrom(src => src.Fechamodificacion))
                .ForMember(dest => dest.ModificadoPor, act => act.MapFrom(src => src.Modificadopor))
                .ForMember(dest => dest.OrderCode, act => act.MapFrom(src => src.OrderCode))
                .ForMember(dest => dest.OrderIndex, act => act.MapFrom(src => src.OrderIndex))
                .ForMember(dest => dest.Qty, act => act.MapFrom(src => src.Qty)).ReverseMap();

            CreateMap<ChangingTablesArtifact, ChangingTablesArtifactDto>()
                .ForMember(dest => dest.ColumnTitle, act => act.MapFrom(src => src.ColumnTitle))
                .ForMember(dest => dest.ColumnTypeId, act => act.MapFrom(src => src.ColumnTypeId))
                .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
                .ForMember(dest => dest.Deriv2Other, act => act.MapFrom(src => src.Deriv2Other))
                .ForMember(dest => dest.DerivId, act => act.MapFrom(src => src.DerivId))
                .ForMember(dest => dest.DerivId2, act => act.MapFrom(src => src.DerivId2))
                .ForMember(dest => dest.DerivOther, act => act.MapFrom(src => src.DerivOther))
                .ForMember(dest => dest.FechaCreacion, act => act.MapFrom(src => src.Fechacreacion))
                .ForMember(dest => dest.FechaModificacion, act => act.MapFrom(src => src.Fechamodificacion))
                .ForMember(dest => dest.FlagRcbnFcbn, act => act.MapFrom(src => src.FlagRcbnFcbn))
                .ForMember(dest => dest.ModificadoPor, act => act.MapFrom(src => src.Modificadopor))
                .ForMember(dest => dest.OperationId, act => act.MapFrom(src => src.OperationId))
                .ForMember(dest => dest.OrderCode, act => act.MapFrom(src => src.OrderCode))
                .ForMember(dest => dest.OrderIndex, act => act.MapFrom(src => src.OrderIndex))
                .ForMember(dest => dest.Taps, act => act.MapFrom(src => src.Taps)).ReverseMap();


            CreateMap<LabTestsArtifact, LabTestsArtifactDto>()
                .ForMember(dest => dest.CreadoPor, act => act.MapFrom(src => src.Creadopor))
                .ForMember(dest => dest.FechaCreacion, act => act.MapFrom(src => src.Fechacreacion))
                .ForMember(dest => dest.FechaModificacion, act => act.MapFrom(src => src.Fechamodificacion))
                .ForMember(dest => dest.ModificadoPor, act => act.MapFrom(src => src.Modificadopor))
                .ForMember(dest => dest.OrderCode, act => act.MapFrom(src => src.OrderCode))
                .ForMember(dest => dest.TextTestDielectric, act => act.MapFrom(src => src.TextTestDielectric))
                .ForMember(dest => dest.TextTestPrototype, act => act.MapFrom(src => src.TextTestPrototype))
                .ForMember(dest => dest.TextTestRoutine, act => act.MapFrom(src => src.TextTestRoutine)).ReverseMap();

            CreateMap<RulesArtifact, RulesArtifactDto>()
              .ForMember(dest => dest.CreadoPor, act => act.MapFrom(src => src.Creadopor))
              .ForMember(dest => dest.FechaCreacion, act => act.MapFrom(src => src.Fechacreacion))
              .ForMember(dest => dest.FechaModificacion, act => act.MapFrom(src => src.Fechamodificacion))
              .ForMember(dest => dest.ModificadoPor, act => act.MapFrom(src => src.Modificadopor))
              .ForMember(dest => dest.OrderCode, act => act.MapFrom(src => src.OrderCode))
              .ForMember(dest => dest.Secuencia, act => act.MapFrom(src => src.Secuencia)).ReverseMap();

            CreateMap<TapBaan, TapBaanDto>()
           .ForMember(dest => dest.CantidadInfBc, act => act.MapFrom(src => src.CantidadInfBc))
           .ForMember(dest => dest.CantidadInfSc, act => act.MapFrom(src => src.CantidadInfSc))
           .ForMember(dest => dest.CantidadSupBc, act => act.MapFrom(src => src.CantidadSupBc))
           .ForMember(dest => dest.CantidadSupSc, act => act.MapFrom(src => src.CantidadSupSc))
           .ForMember(dest => dest.ComboNumericBc, act => act.MapFrom(src => src.ComboNumericBc))
           .ForMember(dest => dest.ComboNumericSc, act => act.MapFrom(src => src.ComboNumericSc))
           .ForMember(dest => dest.CreadoPor, act => act.MapFrom(src => src.Creadopor))
           .ForMember(dest => dest.FechaCreacion, act => act.MapFrom(src => src.Fechacreacion))
           .ForMember(dest => dest.FechaModificacion, act => act.MapFrom(src => src.Fechamodificacion))
           .ForMember(dest => dest.IdentificacionBc, act => act.MapFrom(src => src.IdentificacionBc))
           .ForMember(dest => dest.IdentificacionSc, act => act.MapFrom(src => src.IdentificacionSc))
           .ForMember(dest => dest.InvertidoBc, act => act.MapFrom(src => src.InvertidoBc))
           .ForMember(dest => dest.InvertidoSc, act => act.MapFrom(src => src.InvertidoSc))
           .ForMember(dest => dest.ModificadoPor, act => act.MapFrom(src => src.Modificadopor))
           .ForMember(dest => dest.NominalBc, act => act.MapFrom(src => src.NominalBc))
           .ForMember(dest => dest.NominalSc, act => act.MapFrom(src => src.NominalSc))
           .ForMember(dest => dest.OrderCode, act => act.MapFrom(src => src.OrderCode))
           .ForMember(dest => dest.PorcentajeInfBc, act => act.MapFrom(src => src.PorcentajeInfBc))
           .ForMember(dest => dest.PorcentajeInfSc, act => act.MapFrom(src => src.PorcentajeInfSc))
           .ForMember(dest => dest.PorcentajeSupBc, act => act.MapFrom(src => src.PorcentajeSupBc))
           .ForMember(dest => dest.PorcentajeSupSc, act => act.MapFrom(src => src.PorcentajeSupSc)).ReverseMap();

            CreateMap<NBAINeutro, NBAINeutroDto>()
                .ForMember(dest => dest.ValorNbaiNeutroAltaTension, act => act.MapFrom(src => src.valornbaineutroaltatension))
                .ForMember(dest => dest.ValorNbaiNeutroBajaTension, act => act.MapFrom(src => src.valornbaineutrobajatension))
                .ForMember(dest => dest.ValorNbaiNeutroSegundaBaja, act => act.MapFrom(src => src.valornbaineutrosegundabaja))
                .ForMember(dest => dest.ValorNbaiNeutroTercera, act => act.MapFrom(src => src.valornbaineutrotercera)).ReverseMap();

            CreateMap<ConnectionTypes, ConnectionTypesDto>()
              .ForMember(dest => dest.ConexionAltaTension, act => act.MapFrom(src => src.conexionaltatension))
              .ForMember(dest => dest.ConexionBajaTension, act => act.MapFrom(src => src.conexionbajatension))
              .ForMember(dest => dest.ConexionSegundaBaja, act => act.MapFrom(src => src.conexionsegundabaja))
              .ForMember(dest => dest.ConexionTercera, act => act.MapFrom(src => src.conexiontercera))
              .ForMember(dest => dest.IdConexionAltaTension, act => act.MapFrom(src => src.idconexionaltatension))
              .ForMember(dest => dest.IdConexionBajaTension, act => act.MapFrom(src => src.idconexionbajatension))
              .ForMember(dest => dest.IdConexionSegundaBaja, act => act.MapFrom(src => src.idconexionsegundabaja))
              .ForMember(dest => dest.IdConexionTercera, act => act.MapFrom(src => src.idconexiontercera))
              .ForMember(dest => dest.OtraConexionAltaTension, act => act.MapFrom(src => src.otraconexionaltatension))
              .ForMember(dest => dest.OtraConexionBajaTension, act => act.MapFrom(src => src.otraconexionbajatension))
              .ForMember(dest => dest.OtraConexionSegundaBaja, act => act.MapFrom(src => src.otraconexionsegundabaja))
              .ForMember(dest => dest.OtraConexionTercera, act => act.MapFrom(src => src.otraconexiontercera)).ReverseMap();

            CreateMap<Derivations, DerivationsDto>()
             .ForMember(dest => dest.ConexionEquivalente, act => act.MapFrom(src => src.conexionequivalente))
             .ForMember(dest => dest.ConexionEquivalente_2, act => act.MapFrom(src => src.conexionequivalente_2))
             .ForMember(dest => dest.ConexionEquivalente_3, act => act.MapFrom(src => src.conexionequivalente_3))
             .ForMember(dest => dest.ConexionEquivalente_4, act => act.MapFrom(src => src.conexionequivalente_4))
             .ForMember(dest => dest.IdConexionEquivalente, act => act.MapFrom(src => src.idConexionEquivalente))
             .ForMember(dest => dest.IdConexionEquivalente2, act => act.MapFrom(src => src.idConexionEquivalente2))
             .ForMember(dest => dest.IdConexionEquivalente3, act => act.MapFrom(src => src.idConexionEquivalente3))
             .ForMember(dest => dest.IdConexionEquivalente4, act => act.MapFrom(src => src.idConexionEquivalente4))
             .ForMember(dest => dest.TipoDerivacionAltaTension, act => act.MapFrom(src => src.tipoderivacionaltatension))
             .ForMember(dest => dest.TipoDerivacionAltaTension_2, act => act.MapFrom(src => src.tipoderivacionaltatension_2))
             .ForMember(dest => dest.TipoDerivacionBajaTension, act => act.MapFrom(src => src.tipoderivacionbajatension))
             .ForMember(dest => dest.TipoDerivacionSegundaTension, act => act.MapFrom(src => src.tipoderivacionsegundatension))
             .ForMember(dest => dest.TipoDerivacionTerceraTension, act => act.MapFrom(src => src.tipoderivacionterceratension))
             .ForMember(dest => dest.ValorDerivacionDownAltaTension, act => act.MapFrom(src => src.valorderivaciondownaltatension))
             .ForMember(dest => dest.ValorDerivacionDownAltaTension_2, act => act.MapFrom(src => src.valorderivaciondownaltatension_2))
             .ForMember(dest => dest.ValorDerivacionDownBajaTension, act => act.MapFrom(src => src.valorderivaciondownbajatension))
             .ForMember(dest => dest.ValorDerivacionDownSegundaTension, act => act.MapFrom(src => src.valorderivaciondownsegundatension))
             .ForMember(dest => dest.ValorDerivacionDownTerceraTension, act => act.MapFrom(src => src.valorderivaciondownterceratension))
             .ForMember(dest => dest.ValorDerivacionUpAltaTension, act => act.MapFrom(src => src.valorderivacionupaltatension))
             .ForMember(dest => dest.ValorDerivacionUpAltaTension_2, act => act.MapFrom(src => src.valorderivacionupaltatension_2))
             .ForMember(dest => dest.ValorDerivacionUpBajaTension, act => act.MapFrom(src => src.valorderivacionupbajatension))
             .ForMember(dest => dest.ValorDerivacionUpSegundaTension, act => act.MapFrom(src => src.valorderivacionupsegundatension))
             .ForMember(dest => dest.ValorDerivacionUpTerceraTension, act => act.MapFrom(src => src.valorderivacionupterceratension)).ReverseMap();

            CreateMap<NBAIBilKv, NBAIBilKvDto>()
              .ForMember(dest => dest.NabaiTercera, act => act.MapFrom(src => src.nabaitercera))
              .ForMember(dest => dest.NbaiAltaTension, act => act.MapFrom(src => src.nbaialtatension))
              .ForMember(dest => dest.NbaiBajaTension, act => act.MapFrom(src => src.nbaibajatension))
              .ForMember(dest => dest.NbaiSegundaBaja, act => act.MapFrom(src => src.nbaisegundabaja)).ReverseMap();

            CreateMap<Taps, TapsDto>()
            .ForMember(dest => dest.TapsAltaTension, act => act.MapFrom(src => src.tapsaltatension))
            .ForMember(dest => dest.TapsAltaTension_2, act => act.MapFrom(src => src.tapsaltatension_2))
            .ForMember(dest => dest.TapsBajaTension, act => act.MapFrom(src => src.tapsbajatension))
            .ForMember(dest => dest.TapsSegundaBaja, act => act.MapFrom(src => src.tapssegundabaja))
            .ForMember(dest => dest.TapsTerciario, act => act.MapFrom(src => src.tapsterciario)).ReverseMap();

            CreateMap<VoltageKV, VoltageKVDto>()
          .ForMember(dest => dest.NoSerie, act => act.MapFrom(src => src.noSerie))
          .ForMember(dest => dest.TensionKvAltaTension1, act => act.MapFrom(src => src.tensionkvaltatension1))
          .ForMember(dest => dest.TensionKvAltaTension2, act => act.MapFrom(src => src.tensionkvaltatension2))
          .ForMember(dest => dest.TensionKvAltaTension3, act => act.MapFrom(src => src.tensionkvaltatension3))
          .ForMember(dest => dest.TensionKvAltaTension4, act => act.MapFrom(src => src.tensionkvaltatension4))
          .ForMember(dest => dest.TensionKvBajaTension1, act => act.MapFrom(src => src.tensionkvbajatension1))
          .ForMember(dest => dest.TensionKvBajaTension2, act => act.MapFrom(src => src.tensionkvbajatension2))
          .ForMember(dest => dest.TensionKvBajaTension3, act => act.MapFrom(src => src.tensionkvbajatension3))
          .ForMember(dest => dest.TensionKvBajaTension4, act => act.MapFrom(src => src.tensionkvbajatension4))
          .ForMember(dest => dest.TensionKvSegundaBaja1, act => act.MapFrom(src => src.tensionkvsegundabaja1))
          .ForMember(dest => dest.TensionKvSegundaBaja2, act => act.MapFrom(src => src.tensionkvsegundabaja2))
          .ForMember(dest => dest.TensionKvSegundaBaja3, act => act.MapFrom(src => src.tensionkvsegundabaja3))
          .ForMember(dest => dest.TensionKvSegundaBaja4, act => act.MapFrom(src => src.tensionkvsegundabaja4))
          .ForMember(dest => dest.TensionKvTerciario1, act => act.MapFrom(src => src.tensionkvterciario1))
          .ForMember(dest => dest.TensionKvTerciario2, act => act.MapFrom(src => src.tensionkvterciario2))
          .ForMember(dest => dest.TensionKvTerciario3, act => act.MapFrom(src => src.tensionkvterciario3))
          .ForMember(dest => dest.TensionKvTerciario4, act => act.MapFrom(src => src.tensionkvterciario4)).ReverseMap();

            _ = CreateMap<GeneralArtifact, SplInfoaparatoDg>().ReverseMap();
            _ = CreateMap<NozzlesArtifact, SplInfoaparatoBoq>().ReverseMap();
            _ = CreateMap<CharacteristicsArtifact, SplInfoaparatoCap>().ReverseMap();
            _ = CreateMap<WarrantiesArtifact, SplInfoaparatoGar>().ReverseMap();
            _ = CreateMap<LightningRodArtifact, SplInfoaparatoApr>().ReverseMap();
            _ = CreateMap<ChangingTablesArtifact, SplInfoaparatoCam>().ReverseMap();
            _ = CreateMap<LabTestsArtifact, SplInfoaparatoLab>().ReverseMap();
            _ = CreateMap<RulesArtifact, SplInfoaparatoNor>().ReverseMap();


            _ = CreateMap<InfoCarLocal, SplInfoaparatoCar>().ReverseMap();
            _ = CreateMap<TapBaan, SplInfoaparatoTap>().ReverseMap();


            _ = this.CreateMap<SplPrueba, Tests>().ReverseMap();
            _ = this.CreateMap<Tests, TestsDto>().ReverseMap();

            _ = this.CreateMap<ErrorColumnsDto, ErrorColumns>().ReverseMap();
            _ = this.CreateMap<ResultRADTests, ResultRADTestsDto>().ReverseMap();
            _ = this.CreateMap<RADTests, RADTestsDto>().ReverseMap();
            _ = this.CreateMap<Column, ColumnDto>().ReverseMap();
            _ = this.CreateMap<ResultRADTestsDetails, ResultRADTestsDetailsDto>().ReverseMap();

            _ = this.CreateMap<RDTTests, RDTTestsDto>().ReverseMap();
            _ = this.CreateMap<ResultRDTTestsDetails, ResultRDTTestsDetailsDto>().ReverseMap();

            _ = this.CreateMap<RANTestsDetails, RANTestsDetailsDto>().ReverseMap();
            _ = this.CreateMap<ResultRANTests, ResultRANTestsDto>().ReverseMap();

            _ = this.CreateMap<RANTestsDetailsRADto, RANTestsDetailsRA>().ReverseMap();
            _ = this.CreateMap<RANTestsDetailsTADto, RANTestsDetailsTA>().ReverseMap();

            _ = this.CreateMap<FPCTestsDetailsDto, FPCTestsDetails>().ReverseMap();
            _ = this.CreateMap<FPCTestsDto, FPCTests>().ReverseMap();
            _ = this.CreateMap<FPCTestsValidationsCapDto, FPCTestsValidationsCap>().ReverseMap();
            _ = this.CreateMap<ResultFPCTestsDto, ResultFPCTests>().ReverseMap();
            _ = this.CreateMap<CorrectionFactorSpecificationDto, CorrectionFactorSpecification>().ReverseMap();

            _ = this.CreateMap<FPBTestsDetailsDto, FPBTestsDetails>().ReverseMap();
            _ = this.CreateMap<FPBTestsDto, FPBTests>().ReverseMap();
            _ = this.CreateMap<ResultFPBTestsDto, ResultFPBTests>().ReverseMap();
            _ = this.CreateMap<RecordNozzleInformationDto, RecordNozzleInformation>().ReverseMap();
            _ = this.CreateMap<CorrectionFactorsXMarksXTypesDto, CorrectionFactorsXMarksXTypes>().ReverseMap();

            _ = this.CreateMap<ResistDesignDto, ResistDesign>().ReverseMap();
            _ = this.CreateMap<RODTestsDetailsDto, RODTestsDetails>().ReverseMap();
            _ = this.CreateMap<RODTestsDto, RODTests>().ReverseMap();
            _ = this.CreateMap<ResultRODTestsDto, ResultRODTests>().ReverseMap();

            _ = this.CreateMap<RCTTestsDetailsDto, RCTTestsDetails>().ReverseMap();
            _ = this.CreateMap<RCTTestsDto, RCTTests>().ReverseMap();
            _ = this.CreateMap<ResultRCTTestsDto, ResultRCTTests>().ReverseMap();

            _ = this.CreateMap<ResultPCITestDto, PCITestResponse>().ReverseMap();
            //_ = this.CreateMap<PCIOutTestsDto, PCIOutTests>().ReverseMap();
            //_ = this.CreateMap<PCITestsDetailsDto, PCITestDetail>().ReverseMap();  
            //_ = this.CreateMap<PCITestRatingDto, PCITestRating>().ReverseMap();
            //_ = this.CreateMap<PCITestResultDto, PCISecondaryPositionTest>().ReverseMap();

            _ = this.CreateMap<WarrantiesArtifactDto, WarrantiesArtifact>().ReverseMap();
            _ = this.CreateMap<RODTestsGeneralDto, RODTestsGeneral>().ReverseMap();
            _ = this.CreateMap<PCETestsGeneralDto, PCETestsGeneral>().ReverseMap();
            _ = this.CreateMap<PlateTensionDto, PlateTension>().ReverseMap();

            _ = this.CreateMap<PCETestsDetailsDto, PCETestsDetails>().ReverseMap();
            _ = this.CreateMap<PCETestsDto, PCETests>().ReverseMap();
            _ = this.CreateMap<ResultPCETestsDto, ResultPCETests>().ReverseMap();

            _ = this.CreateMap<PLRTestsDetailsDto, PLRTestsDetails>().ReverseMap();
            _ = this.CreateMap<PLRTestsDto, PLRTests>().ReverseMap();
            _ = this.CreateMap<ResultPLRTestsDto, ResultPLRTests>().ReverseMap();

            _ = this.CreateMap<PRDTestsDetailsDto, PRDTestsDetails>().ReverseMap();
            _ = this.CreateMap<PRDTestsDto, PRDTests>().ReverseMap();
            _ = this.CreateMap<ResultPRDTestsDto, ResultPRDTests>().ReverseMap();

            _ = this.CreateMap<PEETestsDetailsDto, PEETestsDetails>().ReverseMap();
            _ = this.CreateMap<PEETestsDto, PEETests>().ReverseMap();
            _ = this.CreateMap<ResultPEETestsDto, ResultPEETests>().ReverseMap();


            _ = this.CreateMap<ISZTestsDetailsDto, ISZTestsDetails>().ReverseMap();
            _ = this.CreateMap<ISZTestsDetailsDto, ISZTestsDetails>().ReverseMap();
      
            _ = this.CreateMap<ResultISZTestsDto, ResultISZTests>().ReverseMap();
            _ = this.CreateMap<OutISZTestsDto, OutISZTests>().ReverseMap();
            _ = this.CreateMap<SeccionesISZTestDetailsDto, SeccionesISZTestDetails>().ReverseMap();

            
            _ = this.CreateMap<ValidationTestsIsz, ValidationTestsIszDto>().ReverseMap();


            _ = this.CreateMap<RYETestsDetailsDto, RYETestsDetails>().ReverseMap();
            _ = this.CreateMap<ResultRYETestsDto, ResultRYETests>().ReverseMap();
            _ = this.CreateMap<OutRYETestsDto, OutRYETests>().ReverseMap();


            _ = this.CreateMap<TAPTestsDetailsDto, TAPTestsDetails>().ReverseMap();
            _ = this.CreateMap<ResultTAPTestsDto, ResultTAPTests>().ReverseMap();
            _ = this.CreateMap<TAPTestsDto, TAPTests>().ReverseMap();


            _ = this.CreateMap<ResultTDPTestsDto, ResultTDPTests>().ReverseMap();
            _ = this.CreateMap<TDPTerminalsDto, TDPTerminals>().ReverseMap();
            _ = this.CreateMap<TDPTestsDetailsDto, TDPTestsDetails>().ReverseMap();
            _ = this.CreateMap<TDPTestsDto, TDPTests>().ReverseMap();
            _ = this.CreateMap<TDPTestsGeneralDto, TDPTestsGeneral>().ReverseMap();
            _ = this.CreateMap<TDPValid20Dto, TDPValid20>().ReverseMap();



            _ = this.CreateMap<CGDTestsDetailsDto, CGDTestsDetails>().ReverseMap();
            _ = this.CreateMap<CGDTestsDto, CGDTests>().ReverseMap();
            _ = this.CreateMap<ResultCGDTestsDto, ResultCGDTests>().ReverseMap();


            _ = this.CreateMap<OutRDDTestsDto, OutRDDTests>().ReverseMap();
            _ = this.CreateMap<RDDTestsDetailsDto, RDDTestsDetails>().ReverseMap();
            _ = this.CreateMap<RDDTestsGeneralDto, RDDTestsGeneral>().ReverseMap();
            _ = this.CreateMap<ResultRDDTestsDto, ResultRDDTests>().ReverseMap();


            _ = this.CreateMap<FPADielectricStrengthDto, FPADielectricStrength>().ReverseMap();
            _ = this.CreateMap<FPAGasContentDto, FPAGasContent>().ReverseMap();
            _ = this.CreateMap<FPAPowerFactorDto, FPAPowerFactor>().ReverseMap();
            _ = this.CreateMap<FPATestsDetailsDto, FPATestsDetails>().ReverseMap();
            _ = this.CreateMap<FPATestsDto, FPATests>().ReverseMap();
            _ = this.CreateMap<ResultFPATestsDto, ResultFPATests>().ReverseMap();
            _ = this.CreateMap<FPAWaterContentDto, FPAWaterContent>().ReverseMap();


            _ = this.CreateMap<MatrixNRATestsDto, MatrixNRATests>().ReverseMap();
            _ = this.CreateMap<NRATestsDetailsOctDto, NRATestsDetailsOct>().ReverseMap();
            _ = this.CreateMap<NRATestsDetailsRuiDto, NRATestsDetailsRui>().ReverseMap();
            _ = this.CreateMap<NRATestsDto, NRATests>().ReverseMap();
            _ = this.CreateMap<MatrixOneDto, MatrixOne>().ReverseMap();
            _ = this.CreateMap<MatrixTwoDto, MatrixTwo>().ReverseMap();
            _ = this.CreateMap<NRATestsGeneralDto, NRATestsGeneral>().ReverseMap();
            _ = this.CreateMap<ResultNRATestsDto, ResultNRATests>().ReverseMap();


            _ = this.CreateMap<ResultStabilizationDataTestsDto, ResultStabilizationDataTests>().ReverseMap();
            _ = this.CreateMap<StabilizationDataDto, StabilizationData>().ReverseMap();
            _ = this.CreateMap<StabilizationDetailsDataDto, StabilizationDetailsData>().ReverseMap();



            _ = this.CreateMap<ResultDPRTestsDto, ResultDPRTests>().ReverseMap();
            _ = this.CreateMap<DPRTerminalsDto, DPRTerminals>().ReverseMap();
            _ = this.CreateMap<DPRTestsDetailsDto, DPRTestsDetails>().ReverseMap();
            _ = this.CreateMap<DPRTestsDto,DPRTests>().ReverseMap();
            _ = this.CreateMap<DPRTestsGeneralDto, DPRTestsGeneral>().ReverseMap();
            _ = this.CreateMap<DPRValid20Dto, DPRValid20>().ReverseMap();



            _ = this.CreateMap<ETDTestsDetailsDto, ETDTestsDetails>().ReverseMap();
            _ = this.CreateMap<ETDTestsDto, ETDTests>().ReverseMap();
            _ = this.CreateMap<ETDTestsGeneralDto, ETDTestsGeneral>().ReverseMap();
            _ = this.CreateMap<ResultETDTestsDto, ResultETDTests>().ReverseMap();




            _ = this.CreateMap<DetailCuttingDataDto, Domain.SPL.Tests.DetailCuttingData>().ReverseMap();
            _ = this.CreateMap<HeaderCuttingDataDto, Domain.SPL.Tests.HeaderCuttingData>().ReverseMap();
            _ = this.CreateMap<SectionCuttingDataDto, Domain.SPL.Tests.SectionCuttingData>().ReverseMap();
            _ = this.CreateMap<ResultCuttingDataTestsDto, Domain.SPL.Tests.ResultCuttingDataTests>().ReverseMap();
        }
    }
}
