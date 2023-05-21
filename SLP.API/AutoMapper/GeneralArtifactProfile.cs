using AutoMapper;
using SPL.Artifact.Api.DTOs;
using SPL.Artifact.Api.DTOs.Artifactdesign;
using SPL.Artifact.Api.DTOs.BaseTemplate;
using SPL.Artifact.Api.DTOs.Nozzles;
using SPL.Artifact.Api.DTOs.PlateTension;
using SPL.Artifact.Infrastructure.Entities;
using SPL.Domain.SPL.Artifact.ArtifactDesign;
using SPL.Domain.SPL.Artifact.Nozzles;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPL.Artifact.Api.AutoMapper
{
    public class GeneralArtifactProfile : Profile
    {
        public GeneralArtifactProfile()
        {
            _ = CreateMap<InformationArtifactDto,InformationArtifact>().ReverseMap();

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

            _ = CreateMap<SPL.Domain.SPL.Artifact.PlateTension.PlateTension, SplTensionPlaca>().ReverseMap();

            _ = CreateMap<SPL.Domain.SPL.Artifact.PlateTension.PlateTension, PlateTensionDto >().ReverseMap();

            _ = CreateMap<InfoCarLocal, CharacteristicsPlaneTensionDto>().ReverseMap();

            _ = CreateMap<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate, SplPlantillaBase>().ReverseMap();

            _ = CreateMap<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate, BaseTemplateDto>().ReverseMap();

            CreateMap<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate, BaseTemplateDto>()
                              .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
                              .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
                              .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
                              .ForMember(dest => dest.Plantilla, act => act.MapFrom(src => Convert.ToBase64String(src.Plantilla)))
                              .ForMember(dest => dest.ColumnasConfigurables, act => act.MapFrom(src => src.ColumnasConfigurables))
                              .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
                              .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
                              .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
                              .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion));

            CreateMap<BaseTemplateDto, SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplate>()
                          .ForMember(dest => dest.TipoReporte, act => act.MapFrom(src => src.TipoReporte))
                          .ForMember(dest => dest.ClavePrueba, act => act.MapFrom(src => src.ClavePrueba))
                          .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
                          .ForMember(dest => dest.Plantilla, act => act.MapFrom(src => DecodeBase64(src.Plantilla)))
                          .ForMember(dest => dest.ColumnasConfigurables, act => act.MapFrom(src => src.ColumnasConfigurables))
                          .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
                          .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
                          .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
                          .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion));

                _ = CreateMap<NozzlesByDesignDto, NozzlesByDesign>().ReverseMap();
                _ = CreateMap<RecordNozzleInformationDto, RecordNozzleInformation>().ReverseMap();

            _ = CreateMap<ResistDesignDto, ResistDesign>().ReverseMap();
            _ = CreateMap<ResistDesign, SplResistDiseno>().ReverseMap();

            CreateMap<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplateConsolidatedReport, BaseTemplateConsolidatedReportDto>()
                  .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
                  .ForMember(dest => dest.Plantilla, act => act.MapFrom(src => Convert.ToBase64String(src.Plantilla)))
                  .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
                  .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
                  .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
                  .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion));


            CreateMap< BaseTemplateConsolidatedReportDto, SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplateConsolidatedReport>()
      .ForMember(dest => dest.ClaveIdioma, act => act.MapFrom(src => src.ClaveIdioma))
      .ForMember(dest => dest.Plantilla, act => act.MapFrom(src => DecodeBase64(src.Plantilla)))
      .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
      .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
      .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
      .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion));

            _ = CreateMap<SPL.Domain.SPL.Artifact.BaseTemplate.BaseTemplateConsolidatedReport, SplRepConsolidado>()
                .ForMember(dest => dest.Archivo, act => act.MapFrom(src => src.Plantilla))
                .ForMember(dest => dest.Idioma, act => act.MapFrom(src => src.ClaveIdioma))
                .ForMember(dest => dest.NombreArchivo, act => act.MapFrom(src => src.NombreArchivo))
                .ForMember(dest => dest.Creadopor, act => act.MapFrom(src => src.Creadopor))
                .ForMember(dest => dest.Fechacreacion, act => act.MapFrom(src => src.Fechacreacion))
                .ForMember(dest => dest.Modificadopor, act => act.MapFrom(src => src.Modificadopor))
                .ForMember(dest => dest.Fechamodificacion, act => act.MapFrom(src => src.Fechamodificacion))
                .ReverseMap();
        }
        private byte[] DecodeBase64(string pBase64)
        {
            string result = pBase64[(pBase64.IndexOf(",") + 1)..];
            return Convert.FromBase64String(result);
        }

    }
}
