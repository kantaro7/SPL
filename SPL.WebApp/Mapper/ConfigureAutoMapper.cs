namespace SPL.WebApp.Mapper
{
    using AutoMapper;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Helpers;
    using SPL.WebApp.ViewModels;

    public class ConfigureAutoMapper : Profile
    {
        public ConfigureAutoMapper()
            : this("MyProfile")
        {
        }
        protected ConfigureAutoMapper(string profileName)
           : base(profileName)
        {
            #region DTO to ViewModels
            _ = this.CreateMap<InformationArtifactDTO, ArtifactRecordViewModel>()
            #region General

                .ForMember(x => x.OrderCode, x => x.MapFrom(y => y.GeneralArtifact.OrderCode))
                .ForMember(x => x.Descripcion, x => x.MapFrom(y => y.GeneralArtifact.Descripcion))
                .ForMember(x => x.Phases, x => x.MapFrom(y => y.GeneralArtifact.Phases))
                .ForMember(x => x.CustomerName, x => x.MapFrom(y => y.GeneralArtifact.CustomerName))
                .ForMember(x => x.Frecuency, x => x.MapFrom(y => y.GeneralArtifact.Frecuency))
                .ForMember(x => x.PoNumeric, x => x.MapFrom(y => y.GeneralArtifact.PoNumeric))
                .ForMember(x => x.AltitudeF1, x => x.MapFrom(y => y.GeneralArtifact.AltitudeF1 != null ? y.GeneralArtifact.AltitudeF1.Value.ToString() : string.Empty))
                .ForMember(x => x.AltitudeF2, x => x.MapFrom(y => y.GeneralArtifact.AltitudeF2))
                .ForMember(x => x.Typetrafoid, x => x.MapFrom(y => y.GeneralArtifact.TypeTrafoId != null ? y.GeneralArtifact.TypeTrafoId.Value.ToString() : string.Empty))
                .ForMember(x => x.TipoUnidad, x => x.MapFrom(y => y.GeneralArtifact.TipoUnidad))
                .ForMember(x => x.ApplicationId, x => x.MapFrom(y => y.GeneralArtifact.Applicationid != null ? y.GeneralArtifact.Applicationid.Value.ToString() : string.Empty))
                .ForMember(x => x.StandardId, x => x.MapFrom(y => y.GeneralArtifact.StandardId != null ? y.GeneralArtifact.StandardId.Value.ToString() : string.Empty))
                .ForMember(x => x.Norma, x => x.MapFrom(y => y.GeneralArtifact.Norma))
                .ForMember(x => x.LanguageId, x => x.MapFrom(y => y.GeneralArtifact.LanguageId != null ? y.GeneralArtifact.LanguageId.Value.ToString() : string.Empty))
                .ForMember(x => x.ClaveIdioma, x => x.MapFrom(y => y.GeneralArtifact.ClaveIdioma))
                .ForMember(x => x.PolarityId, x => x.MapFrom(y => y.GeneralArtifact.PolarityId != null ? y.GeneralArtifact.PolarityId.Value.ToString() : string.Empty))
                .ForMember(x => x.PolarityOther, x => x.MapFrom(y => y.GeneralArtifact.PolarityOther))
                .ForMember(x => x.DesplazamientoAngular, x => x.MapFrom(y => y.GeneralArtifact.DesplazamientoAngular))
                .ForMember(x => x.Creadopor, x => x.MapFrom(y => y.GeneralArtifact.CreadoPor))
                .ForMember(x => x.Fechacreacion, x => x.MapFrom(y => y.GeneralArtifact.FechaCreacion))
                .ForMember(x => x.Modificadopor, x => x.MapFrom(y => y.GeneralArtifact.ModificadoPor))
                .ForMember(x => x.Fechamodificacion, x => x.MapFrom(y => y.GeneralArtifact.FechaModificacion))
                .ForMember(x => x.OilType, x => x.MapFrom(y => y.GeneralArtifact.OilType))
                .ForMember(x => x.OilBrand, x => x.MapFrom(y => y.GeneralArtifact.OilBrand))

            #endregion
            #region Characteristics     

                .ForPath(x => x.CharacteristicsArtifacts, x => x.MapFrom(y => y.CharacteristicsArtifact))
                .ForMember(x => x.TensionKvAltaTension1, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvAltaTension1)))
                .ForMember(x => x.TensionKvAltaTension2, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvAltaTension2)))
                .ForMember(x => x.TensionKvAltaTension3, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvAltaTension3)))
                .ForMember(x => x.TensionKvAltaTension4, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvAltaTension4)))
                .ForMember(x => x.TensionKvBajaTension1, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvBajaTension1)))
                .ForMember(x => x.TensionKvBajaTension2, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvBajaTension2)))
                .ForMember(x => x.TensionKvBajaTension3, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvBajaTension3)))
                .ForMember(x => x.TensionKvBajaTension4, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvBajaTension4)))
                .ForMember(x => x.TensionKvSegundaBaja1, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvSegundaBaja1)))
                .ForMember(x => x.TensionKvSegundaBaja2, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvSegundaBaja2)))
                .ForMember(x => x.TensionKvSegundaBaja3, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvSegundaBaja3)))
                .ForMember(x => x.TensionKvSegundaBaja4, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvSegundaBaja4)))
                .ForMember(x => x.TensionKvTerciario1, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvTerciario1)))
                .ForMember(x => x.TensionKvTerciario2, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvTerciario2)))
                .ForMember(x => x.TensionKvTerciario3, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvTerciario3)))
                .ForMember(x => x.TensionKvTerciario4, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageKV.TensionKvTerciario4)))
                .ForMember(x => x.NbaiAltaTension, x => x.MapFrom(y => ParserHelper.DecimalToString(y.NBAI.NbaiAltaTension)))
                .ForMember(x => x.NbaiBajaTension, x => x.MapFrom(y => ParserHelper.DecimalToString(y.NBAI.NbaiBajaTension)))
                .ForMember(x => x.NbaiSegundaBaja, x => x.MapFrom(y => ParserHelper.DecimalToString(y.NBAI.NbaiSegundaBaja)))
                .ForMember(x => x.NabaiTercera, x => x.MapFrom(y => ParserHelper.DecimalToString(y.NBAI.NabaiTercera)))
                .ForMember(x => x.ConexionAltaTension, x => x.MapFrom(y => y.Connections.ConexionAltaTension))
                .ForMember(x => x.IdConexionAltaTension, x => x.MapFrom(y => y.Connections.IdConexionAltaTension.ToString()))
                .ForMember(x => x.OtraConexionAltaTension, x => x.MapFrom(y => y.Connections.OtraConexionAltaTension))
                .ForMember(x => x.ConexionBajaTension, x => x.MapFrom(y => y.Connections.ConexionBajaTension))
                .ForMember(x => x.IdConexionBajaTension, x => x.MapFrom(y => y.Connections.IdConexionBajaTension.ToString()))
                .ForMember(x => x.OtraConexionBajaTension, x => x.MapFrom(y => y.Connections.OtraConexionBajaTension))
                .ForMember(x => x.ConexionSegundaBaja, x => x.MapFrom(y => y.Connections.ConexionSegundaBaja))
                .ForMember(x => x.IdConexionSegundaBaja, x => x.MapFrom(y => y.Connections.IdConexionSegundaBaja.ToString()))
                .ForMember(x => x.OtraConexionSegundaBaja, x => x.MapFrom(y => y.Connections.OtraConexionSegundaBaja))
                .ForMember(x => x.ConexionTercera, x => x.MapFrom(y => y.Connections.ConexionTercera))
                .ForMember(x => x.IdConexionTercera, x => x.MapFrom(y => y.Connections.IdConexionTercera.ToString()))
                .ForMember(x => x.OtraConexionTercera, x => x.MapFrom(y => y.Connections.OtraConexionTercera))
                .ForMember(x => x.TapsAltaTension, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Taps.TapsAltaTension)))
                .ForMember(x => x.TapsBajaTension, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Taps.TapsBajaTension)))
                .ForMember(x => x.TapsSegundaBaja, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Taps.TapsSegundaBaja)))
                .ForMember(x => x.TapsTerciario, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Taps.TapsTerciario)))
                .ForMember(x => x.ValorNbaiNeutroAltaTension, x => x.MapFrom(y => ParserHelper.DecimalToString(y.NBAINeutro.ValorNbaiNeutroAltaTension)))
                .ForMember(x => x.ValorNbaiNeutroBajaTension, x => x.MapFrom(y => ParserHelper.DecimalToString(y.NBAINeutro.ValorNbaiNeutroBajaTension)))
                .ForMember(x => x.ValorNbaiNeutroSegundaBaja, x => x.MapFrom(y => ParserHelper.DecimalToString(y.NBAINeutro.ValorNbaiNeutroSegundaBaja)))
                .ForMember(x => x.ValorNbaiNeutroTercera, x => x.MapFrom(y => ParserHelper.DecimalToString(y.NBAINeutro.ValorNbaiNeutroTercera)))
                .ForPath(x => x.ConexionEquivalente, x => x.MapFrom(y => y.Derivations.ConexionEquivalente))
                .ForPath(x => x.ConexionEquivalente_2, x => x.MapFrom(y => y.Derivations.ConexionEquivalente_2))
                .ForPath(x => x.ConexionEquivalente_3, x => x.MapFrom(y => y.Derivations.ConexionEquivalente_3))
                .ForPath(x => x.ConexionEquivalente_4, x => x.MapFrom(y => y.Derivations.ConexionEquivalente_4))
                .ForPath(x => x.IdConexionEquivalente, x => x.MapFrom(y => y.Derivations.IdConexionEquivalente.ToString()))
                .ForPath(x => x.IdConexionEquivalente2, x => x.MapFrom(y => y.Derivations.IdConexionEquivalente2.ToString()))
                .ForPath(x => x.IdConexionEquivalente3, x => x.MapFrom(y => y.Derivations.IdConexionEquivalente3.ToString()))
                .ForPath(x => x.IdConexionEquivalente4, x => x.MapFrom(y => y.Derivations.IdConexionEquivalente4.ToString()))
                .ForPath(x => x.TipoDerivacionAltaTension, x => x.MapFrom(y => y.Derivations.TipoDerivacionAltaTension))
                .ForPath(x => x.TipoDerivacionAltaTension_2, x => x.MapFrom(y => y.Derivations.TipoDerivacionAltaTension_2))
                .ForPath(x => x.TipoDerivacionBajaTension, x => x.MapFrom(y => y.Derivations.TipoDerivacionBajaTension))
                .ForPath(x => x.TipoDerivacionSegundaTension, x => x.MapFrom(y => y.Derivations.TipoDerivacionSegundaTension))
                .ForPath(x => x.TipoDerivacionTerceraTension, x => x.MapFrom(y => y.Derivations.TipoDerivacionTerceraTension))
                .ForPath(x => x.ValorDerivacionDownAltaTension, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Derivations.ValorDerivacionDownAltaTension)))
                .ForPath(x => x.ValorDerivacionDownAltaTension_2, x => x.MapFrom(y => ParserHelper.IntToString(y.Derivations.ValorDerivacionDownAltaTension_2)))
                .ForPath(x => x.ValorDerivacionDownBajaTension, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Derivations.ValorDerivacionDownBajaTension)))
                .ForPath(x => x.ValorDerivacionDownSegundaTension, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Derivations.ValorDerivacionDownSegundaTension)))
                .ForPath(x => x.ValorDerivacionDownTerceraTension, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Derivations.ValorDerivacionDownTerceraTension)))
                .ForPath(x => x.ValorDerivacionUpAltaTension, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Derivations.ValorDerivacionUpAltaTension)))
                .ForPath(x => x.ValorDerivacionUpAltaTension_2, x => x.MapFrom(y => ParserHelper.IntToString(y.Derivations.ValorDerivacionUpAltaTension_2)))
                .ForPath(x => x.ValorDerivacionUpBajaTension, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Derivations.ValorDerivacionUpBajaTension)))
                .ForPath(x => x.ValorDerivacionUpSegundaTension, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Derivations.ValorDerivacionUpSegundaTension)))
                .ForPath(x => x.ValorDerivacionUpTerceraTension, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Derivations.ValorDerivacionUpTerceraTension)))
            #endregion

            #region Warranties

                .ForMember(x => x.Iexc100, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.Iexc100)))
                .ForMember(x => x.Iexc110, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.Iexc110)))
                .ForMember(x => x.Kwaux1, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.Kwaux1)))
                .ForMember(x => x.Kwaux2, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.Kwaux2)))
                .ForMember(x => x.Kwaux3, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.Kwaux3)))
                .ForMember(x => x.Kwaux4, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.Kwaux4)))
                .ForMember(x => x.Kwcu, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.Kwcu)))
                .ForMember(x => x.KwcuKv, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.KwcuKv)))
                .ForMember(x => x.KwcuMva, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.KwcuMva)))
                .ForMember(x => x.Kwfe100, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.Kwfe100)))
                .ForMember(x => x.Kwfe110, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.Kwfe110)))
                .ForMember(x => x.Kwtot100, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.Kwtot100)))
                .ForMember(x => x.Kwtot110, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.Kwtot110)))
                .ForMember(x => x.NoiseFa1, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.NoiseFa1)))
                .ForMember(x => x.NoiseFa2, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.NoiseFa2)))
                .ForMember(x => x.NoiseOa, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.NoiseOa)))
                .ForMember(x => x.TolerancyKwAux, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.TolerancyKwAux)))
                .ForMember(x => x.TolerancyKwCu, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.TolerancyKwCu)))
                .ForMember(x => x.TolerancyKwfe, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.TolerancyKwfe)))
                .ForMember(x => x.TolerancyKwtot, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.TolerancyKwtot)))
                .ForMember(x => x.TolerancyZpositive, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.TolerancyZpositive)))
                .ForMember(x => x.TolerancyZpositive2, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.TolerancyZpositive2)))
                .ForMember(x => x.ZPositiveHx, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.ZPositiveHx)))
                .ForMember(x => x.ZPositiveHy, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.ZPositiveHy)))
                .ForMember(x => x.ZPositiveMva, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.ZPositiveMva)))
                .ForMember(x => x.ZPositiveXy, x => x.MapFrom(y => ParserHelper.DecimalToString(y.WarrantiesArtifact.ZPositiveXy)))

            #endregion

            #region Nozzles

                .ForMember(x => x.NozzlesArtifacts, x => x.MapFrom(y => y.NozzlesArtifact))
            #endregion

            #region LightningRod

                .ForMember(x => x.LightningRodArtifacts, x => x.MapFrom(y => y.LightningRodArtifact))

            #endregion

            #region Changes

                .ForMember(x => x.ChangingTablesArtifacs, x => x.MapFrom(y => y.ChangingTablesArtifact))

            #endregion

            #region Tabbann

                 .ForMember(x => x.ComboNumericSc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.ComboNumericSc);
                 })
                 .ForMember(x => x.CantidadSupSc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.CantidadSupSc);
                 })
                 .ForMember(x => x.CantidadSupBc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.CantidadSupBc);
                 })

                 .ForMember(x => x.PorcentajeSupSc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.PorcentajeSupSc);
                 })
                 .ForMember(x => x.CantidadInfSc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.CantidadInfSc);
                 })
                 .ForMember(x => x.PorcentajeInfSc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.PorcentajeInfSc);
                 })
                 .ForMember(x => x.NominalSc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.NominalSc);
                 })
                 .ForMember(x => x.IdentificacionSc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.IdentificacionSc);
                 })
                 .ForMember(x => x.ComboNumericBc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.ComboNumericBc);
                 })
                 .ForMember(x => x.PorcentajeSupBc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.PorcentajeSupBc);
                 })
                 .ForMember(x => x.CantidadInfBc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.CantidadInfBc);
                 })
                 .ForMember(x => x.PorcentajeInfBc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.PorcentajeInfBc);
                 })
                 .ForMember(x => x.NominalBc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.NominalBc);
                 })
                 .ForMember(x => x.PorcentajeInfBc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.PorcentajeInfBc);
                 })
                 .ForMember(x => x.NominalBc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.NominalBc);
                 })
                 .ForMember(x => x.IdentificacionBc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.IdentificacionBc);
                 })
                 .ForMember(x => x.InvertidoBc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.InvertidoBc);
                 })
                 .ForMember(x => x.InvertidoSc, x =>
                 {

                     x.Condition(src => (src.TapBaan != null));
                     x.MapFrom(src => src.TapBaan.InvertidoSc);
                 })

            #endregion

            #region LabTests

                .ForMember(x => x.LabTestsArtifact, x => x.MapFrom(y => y.LabTestsArtifact))

            #endregion

            #region Norms

                .ForMember(x => x.RulesArtifacts, x => x.MapFrom(y => y.RulesArtifact));

            #endregion

            _ = this.CreateMap<CharacteristicsArtifactDTO, CharacteristicsArtifactViewModel>()
                .ForPath(x => x.Secuencia, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Secuencia)))
                .ForPath(x => x.OverElevation, x => x.MapFrom(y => ParserHelper.DecimalToString(y.OverElevation)))
                .ForPath(x => x.Hstr, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Hstr)))
                .ForPath(x => x.DevAwr, x => x.MapFrom(y => ParserHelper.DecimalToString(y.DevAwr)))
                .ForPath(x => x.Mvaf1, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Mvaf1)))
                .ForPath(x => x.Mvaf2, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Mvaf2)))
                .ForPath(x => x.Mvaf3, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Mvaf3)))
                .ForPath(x => x.Mvaf4, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Mvaf4)));

            _ = this.CreateMap<NozzlesArtifactDTO, NozzlesArtifactViewModel>()
                .ForPath(x => x.ColumnTypeId, x => x.MapFrom(y => ParserHelper.DecimalToString(y.ColumnTypeId)))
                .ForPath(x => x.OrderIndex, x => x.MapFrom(y => ParserHelper.DecimalToString(y.OrderIndex)))
                .ForPath(x => x.Qty, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Qty)))
                .ForPath(x => x.VoltageClass, x => x.MapFrom(y => ParserHelper.DecimalToString(y.VoltageClass)))
                .ForPath(x => x.BilClass, x => x.MapFrom(y => ParserHelper.DecimalToString(y.BilClass)))
                .ForPath(x => x.CurrentAmps, x => x.MapFrom(y => ParserHelper.DecimalToString(y.CurrentAmps)))
                .ForPath(x => x.CurrentAmpsReq, x => x.MapFrom(y => ParserHelper.DecimalToString(y.CurrentAmpsReq)))
                .ForPath(x => x.CorrienteUnidad, x => x.MapFrom(y => ParserHelper.DecimalToString(y.CorrienteUnidad)));

            _ = this.CreateMap<LightningRodArtifactDTO, LightningRodArtifactViewModel>()
                .ForPath(x => x.ColumnTypeId, x => x.MapFrom(y => ParserHelper.DecimalToString(y.ColumnTypeId)))
                .ForPath(x => x.OrderIndex, x => x.MapFrom(y => ParserHelper.DecimalToString(y.OrderIndex)))
                .ForPath(x => x.Qty, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Qty)));

            _ = this.CreateMap<ChangingTablesArtifactDTO, ChangingTablesArtifacViewModel>()
                .ForPath(x => x.ColumnTypeId, x => x.MapFrom(y => ParserHelper.DecimalToString(y.ColumnTypeId)))
                .ForPath(x => x.OrderIndex, x => x.MapFrom(y => ParserHelper.DecimalToString(y.OrderIndex)))
                .ForPath(x => x.OperationId, x => x.MapFrom(y => ParserHelper.DecimalToString(y.OperationId)))
                .ForPath(x => x.DerivId, x => x.MapFrom(y => ParserHelper.DecimalToString(y.DerivId)))
                .ForPath(x => x.DerivId2, x => x.MapFrom(y => ParserHelper.DecimalToString(y.DerivId2)))
                .ForPath(x => x.Taps, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Taps)));

            _ = this.CreateMap<LabTestsArtifactDTO, LabTestsArtifactViewModel>();

            _ = this.CreateMap<RulesArtifactDTO, RulesArtifactViewModel>()
                .ForPath(x => x.Secuencia, x => x.MapFrom(y => ParserHelper.DecimalToString(y.Secuencia)));

            //CreateMap<TapBaanDTO, TapBaanViewModel>()
            //    .ForMember(x => x.ComboNumericSc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.ComboNumericSc)))
            //    .ForMember(x => x.CantidadSupSc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.CantidadSupSc)))
            //    .ForMember(x => x.PorcentajeSupSc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.PorcentajeSupSc)))
            //    .ForMember(x => x.CantidadInfSc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.CantidadInfSc)))
            //    .ForMember(x => x.PorcentajeInfSc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.PorcentajeInfSc)))
            //    .ForMember(x => x.NominalSc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.NominalSc)))
            //    .ForMember(x => x.IdentificacionSc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.IdentificacionSc)))
            //    .ForMember(x => x.ComboNumericBc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.ComboNumericBc)))
            //    .ForMember(x => x.PorcentajeSupBc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.PorcentajeSupBc)))
            //    .ForMember(x => x.CantidadInfBc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.CantidadInfBc)))
            //    .ForMember(x => x.PorcentajeInfBc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.PorcentajeInfBc)))
            //    .ForMember(x => x.NominalBc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.NominalBc)))
            //    .ForMember(x => x.IdentificacionBc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.IdentificacionBc)))
            //     .ForMember(x => x.InvertidoBc, x => x.MapFrom(y => y.InvertidoBc.Value.Equals(1) ? true : false))
            //     .ForMember(x => x.InvertidoSc, x => x.MapFrom(y => y.InvertidoSc.Value.Equals(1) ? true : false));

            _ = this.CreateMap<TapBaanDTO, ArtifactRecordViewModel>()
            .ForMember(x => x.ComboNumericSc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.ComboNumericSc)))
                .ForMember(x => x.CantidadSupSc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.CantidadSupSc)))
                .ForMember(x => x.PorcentajeSupSc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.PorcentajeSupSc)))
                .ForMember(x => x.CantidadInfSc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.CantidadInfSc)))
                .ForMember(x => x.PorcentajeInfSc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.PorcentajeInfSc)))
                .ForMember(x => x.NominalSc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.NominalSc)))
                .ForMember(x => x.IdentificacionSc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.IdentificacionSc)))
                .ForMember(x => x.ComboNumericBc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.ComboNumericBc)))
                .ForMember(x => x.PorcentajeSupBc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.PorcentajeSupBc)))
                .ForMember(x => x.CantidadInfBc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.CantidadInfBc)))
                .ForMember(x => x.PorcentajeInfBc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.PorcentajeInfBc)))
                .ForMember(x => x.NominalBc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.NominalBc)))
                .ForMember(x => x.IdentificacionBc, x => x.MapFrom(y => ParserHelper.DecimalToString(y.IdentificacionBc)))
                 .ForPath(x => x.InvertidoBc, x => x.MapFrom(y => ParserHelper.DecimalToBool(y.InvertidoBc)))
                 .ForPath(x => x.InvertidoSc, x => x.MapFrom(y => ParserHelper.DecimalToBool(y.InvertidoSc)));
            #endregion

            #region ViewModel to DTO

            _ = this.CreateMap<CharacteristicsArtifactViewModel, CharacteristicsArtifactDTO>()
                .ForPath(x => x.Secuencia, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.Secuencia)))
                .ForPath(x => x.OverElevation, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.OverElevation)))
                .ForPath(x => x.Hstr, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.Hstr)))
                .ForPath(x => x.DevAwr, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.DevAwr)))
                .ForPath(x => x.Mvaf1, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.Mvaf1)))
                .ForPath(x => x.Mvaf2, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.Mvaf2)))
                .ForPath(x => x.Mvaf3, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.Mvaf3)))
                .ForPath(x => x.Mvaf4, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.Mvaf4)));

            _ = this.CreateMap<NozzlesArtifactViewModel, NozzlesArtifactDTO>()
                .ForPath(x => x.ColumnTypeId, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.ColumnTypeId)))
                .ForPath(x => x.OrderIndex, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.OrderIndex)))
                .ForPath(x => x.Qty, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.Qty)))
                .ForPath(x => x.VoltageClass, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.VoltageClass)))
                .ForPath(x => x.BilClass, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.BilClass)))
                .ForPath(x => x.CurrentAmps, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.CurrentAmps)))
                .ForPath(x => x.CurrentAmpsReq, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.CurrentAmpsReq)))
                .ForPath(x => x.CorrienteUnidad, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.CorrienteUnidad)));

            _ = this.CreateMap<LightningRodArtifactViewModel, LightningRodArtifactDTO>()
                .ForPath(x => x.ColumnTypeId, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.ColumnTypeId)))
                .ForPath(x => x.OrderIndex, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.OrderIndex)))
                .ForPath(x => x.Qty, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.Qty)));

            _ = this.CreateMap<ChangingTablesArtifacViewModel, ChangingTablesArtifactDTO>()
                .ForPath(x => x.ColumnTypeId, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.ColumnTypeId)))
                .ForPath(x => x.OrderIndex, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.OrderIndex)))
                .ForPath(x => x.OperationId, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.OperationId)))
                .ForPath(x => x.DerivId, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.DerivId)))
                .ForPath(x => x.DerivId2, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.DerivId2)))
                .ForPath(x => x.Taps, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.Taps)));

            _ = this.CreateMap<LabTestsArtifactViewModel, LabTestsArtifactDTO>();

            _ = this.CreateMap<RulesArtifactViewModel, RulesArtifactDTO>()
                .ForPath(x => x.Secuencia, x => x.MapFrom(y => ParserHelper.StringToDecimal(y.Secuencia)));

            //CreateMap<TapBaanViewModel, TapBaanDTO>()
            //    .ForPath(x => x.ComboNumericSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ComboNumericSc)))
            //    .ForPath(x => x.CantidadSupSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.CantidadSupSc)))
            //    .ForPath(x => x.PorcentajeSupSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.PorcentajeSupSc)))
            //    .ForPath(x => x.CantidadInfSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.CantidadInfSc)))
            //    .ForPath(x => x.PorcentajeInfSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.PorcentajeInfSc)))
            //    .ForPath(x => x.NominalSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.NominalSc)))
            //    .ForPath(x => x.IdentificacionSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.IdentificacionSc)))
            //    .ForPath(x => x.ComboNumericBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ComboNumericBc)))
            //    .ForPath(x => x.PorcentajeSupBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.PorcentajeSupBc)))
            //    .ForPath(x => x.CantidadInfBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.CantidadInfBc)))
            //    .ForPath(x => x.PorcentajeInfBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.PorcentajeInfBc)))
            //    .ForPath(x => x.NominalBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.NominalBc)))
            //    .ForMember(x => x.InvertidoBc, x => x.MapFrom(y => y.InvertidoBc.Value ? 1 : 0))
            //    .ForMember(x => x.InvertidoSc, x => x.MapFrom(y => y.InvertidoSc.Value ? 1 : 0));

            _ = this.CreateMap<ArtifactRecordViewModel, InformationArtifactDTO>()
            #region General

                .ForPath(x => x.GeneralArtifact.OrderCode, x => x.MapFrom(y => y.OrderCode))
                .ForPath(x => x.GeneralArtifact.Descripcion, x => x.MapFrom(y => y.Descripcion))
                .ForPath(x => x.GeneralArtifact.Phases, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.Phases)))
                .ForPath(x => x.GeneralArtifact.CustomerName, x => x.MapFrom(y => y.CustomerName))
                .ForPath(x => x.GeneralArtifact.Frecuency, x => x.MapFrom(y => y.Frecuency))
                .ForPath(x => x.GeneralArtifact.PoNumeric, x => x.MapFrom(y => y.PoNumeric))
                .ForPath(x => x.GeneralArtifact.AltitudeF1, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.AltitudeF1)))
                .ForPath(x => x.GeneralArtifact.AltitudeF2, x => x.MapFrom(y => y.AltitudeF2))
                .ForPath(x => x.GeneralArtifact.TypeTrafoId, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.Typetrafoid)))
                .ForPath(x => x.GeneralArtifact.TipoUnidad, x => x.MapFrom(y => y.TipoUnidad))
                .ForPath(x => x.GeneralArtifact.Applicationid, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ApplicationId)))
                .ForPath(x => x.GeneralArtifact.StandardId, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.StandardId)))
                .ForPath(x => x.GeneralArtifact.Norma, x => x.MapFrom(y => y.NormaEquivalente))
                .ForPath(x => x.GeneralArtifact.LanguageId, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.LanguageId)))
                .ForPath(x => x.GeneralArtifact.ClaveIdioma, x => x.MapFrom(y => y.ClaveIdioma))
                .ForPath(x => x.GeneralArtifact.PolarityId, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.PolarityId)))
                .ForPath(x => x.GeneralArtifact.PolarityOther, x => x.MapFrom(y => y.PolarityOther))
                .ForPath(x => x.GeneralArtifact.DesplazamientoAngular, x => x.MapFrom(y => y.DesplazamientoAngular))
                .ForPath(x => x.GeneralArtifact.CreadoPor, x => x.MapFrom(y => y.Creadopor))
                .ForPath(x => x.GeneralArtifact.FechaCreacion, x => x.MapFrom(y => y.Fechacreacion))
                .ForPath(x => x.GeneralArtifact.ModificadoPor, x => x.MapFrom(y => y.Modificadopor))
                .ForPath(x => x.GeneralArtifact.FechaModificacion, x => x.MapFrom(y => y.Fechamodificacion))
                 .ForPath(x => x.GeneralArtifact.OilBrand, x => x.MapFrom(y => y.OilBrand))
                  .ForPath(x => x.GeneralArtifact.OilType, x => x.MapFrom(y => y.OilType))

            #endregion
            #region Characteristics     

                .ForPath(x => x.CharacteristicsArtifact, x => x.MapFrom(y => y.CharacteristicsArtifacts))
                .ForPath(x => x.VoltageKV.TensionKvAltaTension1, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvAltaTension1)))
                .ForPath(x => x.VoltageKV.TensionKvAltaTension2, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvAltaTension2)))
                .ForPath(x => x.VoltageKV.TensionKvAltaTension3, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvAltaTension3)))
                .ForPath(x => x.VoltageKV.TensionKvAltaTension4, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvAltaTension4)))
                .ForPath(x => x.VoltageKV.TensionKvBajaTension1, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvBajaTension1)))
                .ForPath(x => x.VoltageKV.TensionKvBajaTension2, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvBajaTension2)))
                .ForPath(x => x.VoltageKV.TensionKvBajaTension3, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvBajaTension3)))
                .ForPath(x => x.VoltageKV.TensionKvBajaTension4, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvBajaTension4)))
                .ForPath(x => x.VoltageKV.TensionKvSegundaBaja1, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvSegundaBaja1)))
                .ForPath(x => x.VoltageKV.TensionKvSegundaBaja2, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvSegundaBaja2)))
                .ForPath(x => x.VoltageKV.TensionKvSegundaBaja3, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvSegundaBaja3)))
                .ForPath(x => x.VoltageKV.TensionKvSegundaBaja4, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvSegundaBaja4)))
                .ForPath(x => x.VoltageKV.TensionKvTerciario1, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvTerciario1)))
                .ForPath(x => x.VoltageKV.TensionKvTerciario2, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvTerciario2)))
                .ForPath(x => x.VoltageKV.TensionKvTerciario3, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvTerciario3)))
                .ForPath(x => x.VoltageKV.TensionKvTerciario4, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TensionKvTerciario4)))
                .ForPath(x => x.VoltageKV.NoSerie, x => x.MapFrom(y => y.OrderCode))
                .ForPath(x => x.NBAI.NbaiAltaTension, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.NbaiAltaTension)))
                .ForPath(x => x.NBAI.NbaiBajaTension, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.NbaiBajaTension)))
                .ForPath(x => x.NBAI.NbaiSegundaBaja, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.NbaiSegundaBaja)))
                .ForPath(x => x.NBAI.NabaiTercera, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.NabaiTercera)))
                .ForPath(x => x.Connections.ConexionAltaTension, x => x.MapFrom(y => y.ConexionAltaTension))
                .ForPath(x => x.Connections.IdConexionAltaTension, x => x.MapFrom(y => ParserHelper.StringToInt(y.IdConexionAltaTension)))
                .ForPath(x => x.Connections.OtraConexionAltaTension, x => x.MapFrom(y => y.OtraConexionAltaTension))
                .ForPath(x => x.Connections.ConexionBajaTension, x => x.MapFrom(y => y.ConexionBajaTension))
                .ForPath(x => x.Connections.IdConexionBajaTension, x => x.MapFrom(y => ParserHelper.StringToInt(y.IdConexionBajaTension)))
                .ForPath(x => x.Connections.OtraConexionBajaTension, x => x.MapFrom(y => y.OtraConexionBajaTension))
                .ForPath(x => x.Connections.ConexionSegundaBaja, x => x.MapFrom(y => y.ConexionSegundaBaja))
                .ForPath(x => x.Connections.IdConexionSegundaBaja, x => x.MapFrom(y => ParserHelper.StringToInt(y.IdConexionSegundaBaja)))
                .ForPath(x => x.Connections.OtraConexionSegundaBaja, x => x.MapFrom(y => y.OtraConexionSegundaBaja))
                .ForPath(x => x.Connections.ConexionTercera, x => x.MapFrom(y => y.ConexionTercera))
                .ForPath(x => x.Connections.IdConexionTercera, x => x.MapFrom(y => ParserHelper.StringToInt(y.IdConexionTercera)))
                .ForPath(x => x.Connections.OtraConexionTercera, x => x.MapFrom(y => y.OtraConexionTercera))
                .ForPath(x => x.Taps.TapsAltaTension, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TapsAltaTension)))
                .ForPath(x => x.Taps.TapsBajaTension, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TapsBajaTension)))
                .ForPath(x => x.Taps.TapsSegundaBaja, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TapsSegundaBaja)))
                .ForPath(x => x.Taps.TapsTerciario, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TapsTerciario)))
                .ForPath(x => x.NBAINeutro.ValorNbaiNeutroAltaTension, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ValorNbaiNeutroAltaTension)))
                .ForPath(x => x.NBAINeutro.ValorNbaiNeutroBajaTension, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ValorNbaiNeutroBajaTension)))
                .ForPath(x => x.NBAINeutro.ValorNbaiNeutroSegundaBaja, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ValorNbaiNeutroSegundaBaja)))
                .ForPath(x => x.NBAINeutro.ValorNbaiNeutroTercera, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ValorNbaiNeutroTercera)))
                .ForPath(x => x.Derivations.ConexionEquivalente, x => x.MapFrom(y => y.ConexionEquivalente))
                .ForPath(x => x.Derivations.ConexionEquivalente_2, x => x.MapFrom(y => y.ConexionEquivalente_2))
                .ForPath(x => x.Derivations.ConexionEquivalente_3, x => x.MapFrom(y => y.ConexionEquivalente_3))
                .ForPath(x => x.Derivations.ConexionEquivalente_4, x => x.MapFrom(y => y.ConexionEquivalente_4))
                .ForPath(x => x.Derivations.IdConexionEquivalente, x => x.MapFrom(y => ParserHelper.StringToInt(y.IdConexionEquivalente)))
                .ForPath(x => x.Derivations.IdConexionEquivalente2, x => x.MapFrom(y => ParserHelper.StringToInt(y.IdConexionEquivalente2)))
                .ForPath(x => x.Derivations.IdConexionEquivalente3, x => x.MapFrom(y => ParserHelper.StringToInt(y.IdConexionEquivalente3)))
                .ForPath(x => x.Derivations.IdConexionEquivalente4, x => x.MapFrom(y => ParserHelper.StringToInt(y.IdConexionEquivalente4)))
                .ForPath(x => x.Derivations.TipoDerivacionAltaTension, x => x.MapFrom(y => y.TipoDerivacionAltaTension))
                .ForPath(x => x.Derivations.TipoDerivacionAltaTension_2, x => x.MapFrom(y => y.TipoDerivacionAltaTension_2))
                .ForPath(x => x.Derivations.TipoDerivacionBajaTension, x => x.MapFrom(y => y.TipoDerivacionBajaTension))
                .ForPath(x => x.Derivations.TipoDerivacionSegundaTension, x => x.MapFrom(y => y.TipoDerivacionSegundaTension))
                .ForPath(x => x.Derivations.TipoDerivacionTerceraTension, x => x.MapFrom(y => y.TipoDerivacionTerceraTension))
                .ForPath(x => x.Derivations.ValorDerivacionDownAltaTension, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ValorDerivacionDownAltaTension)))
                .ForPath(x => x.Derivations.ValorDerivacionDownAltaTension_2, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ValorDerivacionDownAltaTension_2)))
                .ForPath(x => x.Derivations.ValorDerivacionDownBajaTension, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ValorDerivacionDownBajaTension)))
                .ForPath(x => x.Derivations.ValorDerivacionDownSegundaTension, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ValorDerivacionDownSegundaTension)))
                .ForPath(x => x.Derivations.ValorDerivacionDownTerceraTension, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ValorDerivacionDownTerceraTension)))
                .ForPath(x => x.Derivations.ValorDerivacionUpAltaTension, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ValorDerivacionUpAltaTension)))
                .ForPath(x => x.Derivations.ValorDerivacionUpAltaTension_2, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ValorDerivacionUpAltaTension_2)))
                .ForPath(x => x.Derivations.ValorDerivacionUpBajaTension, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ValorDerivacionUpBajaTension)))
                .ForPath(x => x.Derivations.ValorDerivacionUpSegundaTension, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ValorDerivacionUpSegundaTension)))
                .ForPath(x => x.Derivations.ValorDerivacionUpTerceraTension, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ValorDerivacionUpTerceraTension)))

            #endregion

            #region Warranties

                .ForPath(x => x.WarrantiesArtifact.Iexc100, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.Iexc100)))
                .ForPath(x => x.WarrantiesArtifact.Iexc110, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.Iexc110)))
                .ForPath(x => x.WarrantiesArtifact.Kwaux1, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.Kwaux1)))
                .ForPath(x => x.WarrantiesArtifact.Kwaux2, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.Kwaux2)))
                .ForPath(x => x.WarrantiesArtifact.Kwaux3, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.Kwaux3)))
                .ForPath(x => x.WarrantiesArtifact.Kwaux4, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.Kwaux4)))
                .ForPath(x => x.WarrantiesArtifact.Kwcu, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.Kwcu)))
                .ForPath(x => x.WarrantiesArtifact.KwcuKv, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.KwcuKv)))
                .ForPath(x => x.WarrantiesArtifact.KwcuMva, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.KwcuMva)))
                .ForPath(x => x.WarrantiesArtifact.Kwfe100, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.Kwfe100)))
                .ForPath(x => x.WarrantiesArtifact.Kwfe110, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.Kwfe110)))
                .ForPath(x => x.WarrantiesArtifact.Kwtot100, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.Kwtot100)))
                .ForPath(x => x.WarrantiesArtifact.Kwtot110, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.Kwtot110)))
                .ForPath(x => x.WarrantiesArtifact.NoiseFa1, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.NoiseFa1)))
                .ForPath(x => x.WarrantiesArtifact.NoiseFa2, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.NoiseFa2)))
                .ForPath(x => x.WarrantiesArtifact.NoiseOa, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.NoiseOa)))
                .ForPath(x => x.WarrantiesArtifact.TolerancyKwAux, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TolerancyKwAux)))
                .ForPath(x => x.WarrantiesArtifact.TolerancyKwCu, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TolerancyKwCu)))
                .ForPath(x => x.WarrantiesArtifact.TolerancyKwfe, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TolerancyKwfe)))
                .ForPath(x => x.WarrantiesArtifact.TolerancyKwtot, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TolerancyKwtot)))
                .ForPath(x => x.WarrantiesArtifact.TolerancyZpositive, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TolerancyZpositive)))
                .ForPath(x => x.WarrantiesArtifact.TolerancyZpositive2, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.TolerancyZpositive2)))
                .ForPath(x => x.WarrantiesArtifact.ZPositiveHx, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ZPositiveHx)))
                .ForPath(x => x.WarrantiesArtifact.ZPositiveHy, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ZPositiveHy)))
                .ForPath(x => x.WarrantiesArtifact.ZPositiveMva, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ZPositiveMva)))
                .ForPath(x => x.WarrantiesArtifact.ZPositiveXy, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ZPositiveXy)))
                .ForPath(x => x.WarrantiesArtifact.OrderCode, x => x.MapFrom(y => y.OrderCode))
                .ForPath(x => x.WarrantiesArtifact.FechaCreacion, x => x.MapFrom(y => y.Fechacreacion))
                .ForPath(x => x.WarrantiesArtifact.FechaModificacion, x => x.MapFrom(y => y.Fechamodificacion))
                .ForPath(x => x.WarrantiesArtifact.CreadoPor, x => x.MapFrom(y => y.Creadopor))
                .ForPath(x => x.WarrantiesArtifact.ModificadoPor, x => x.MapFrom(y => y.Modificadopor))

            #endregion

            #region Nozzles

                .ForMember(x => x.NozzlesArtifact, x => x.MapFrom(y => y.NozzlesArtifacts))
            #endregion

            #region LightningRod

                .ForMember(x => x.LightningRodArtifact, x => x.MapFrom(y => y.LightningRodArtifacts))

            #endregion

            #region Changes

                .ForMember(x => x.ChangingTablesArtifact, x => x.MapFrom(y => y.ChangingTablesArtifacs))

            #endregion

            #region Tabbann

                //.ForMember(x => x.TapBaan, x => x.MapFrom(y => y.TapBaan))
                //.ForPath(x => x.TapBaan.ComboNumericSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ComboNumericSc)))
                //.ForPath(x => x.TapBaan.CantidadSupSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.CantidadSupSc)))
                //.ForPath(x => x.TapBaan.PorcentajeSupSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.PorcentajeSupSc)))
                //.ForPath(x => x.TapBaan.CantidadInfSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.CantidadInfSc)))
                //.ForPath(x => x.TapBaan.PorcentajeInfSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.PorcentajeInfSc)))
                //.ForPath(x => x.TapBaan.NominalSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.NominalSc)))
                //.ForPath(x => x.TapBaan.IdentificacionSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.IdentificacionSc)))
                //.ForPath(x => x.TapBaan.ComboNumericBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ComboNumericBc)))
                //.ForPath(x => x.TapBaan.PorcentajeSupBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.PorcentajeSupBc)))
                //.ForPath(x => x.TapBaan.CantidadInfBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.CantidadInfBc)))
                //.ForPath(x => x.TapBaan.PorcentajeInfBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.PorcentajeInfBc)))
                //.ForPath(x => x.TapBaan.NominalBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.NominalBc)))
                //.ForPath(x => x.TapBaan.InvertidoBc, x => x.MapFrom(y => ParserHelper.BoolToDecimal(y.InvertidoBc)))
                //.ForPath(x => x.TapBaan.InvertidoSc, x => x.MapFrom(y => ParserHelper.BoolToDecimal(y.InvertidoSc)))

            #endregion

            #region LabTests

                .ForMember(x => x.LabTestsArtifact, x => x.MapFrom(y => y.LabTestsArtifact))

            #endregion

            #region Norms

                .ForMember(x => x.RulesArtifact, x => x.MapFrom(y => y.RulesArtifacts));

            #endregion
            _ = this.CreateMap<ArtifactRecordViewModel, TapBaanDTO>()
                .ForMember(x => x.ComboNumericSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ComboNumericSc)))
                .ForMember(x => x.CantidadSupSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.CantidadSupSc)))
                .ForMember(x => x.PorcentajeSupSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.PorcentajeSupSc)))
                .ForMember(x => x.CantidadInfSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.CantidadInfSc)))
                .ForMember(x => x.PorcentajeInfSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.PorcentajeInfSc)))
                .ForMember(x => x.NominalSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.NominalSc)))
                .ForMember(x => x.IdentificacionSc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.IdentificacionSc)))
                .ForMember(x => x.ComboNumericBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.ComboNumericBc)))
                .ForMember(x => x.CantidadSupBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.CantidadSupBc)))
                .ForMember(x => x.IdentificacionBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.IdentificacionBc)))
                .ForMember(x => x.PorcentajeSupBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.PorcentajeSupBc)))
                .ForMember(x => x.CantidadInfBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.CantidadInfBc)))
                .ForMember(x => x.PorcentajeInfBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.PorcentajeInfBc)))
                .ForMember(x => x.NominalBc, x => x.MapFrom(y => ParserHelper.StringToDecimalNullable(y.NominalBc)))
                .ForMember(x => x.InvertidoBc, x => x.MapFrom(y => ParserHelper.BoolToDecimal(y.InvertidoBc)))
                .ForMember(x => x.InvertidoSc, x => x.MapFrom(y => ParserHelper.BoolToDecimal(y.InvertidoSc)))
                .ForMember(x => x.FechaCreacion, x => x.MapFrom(y => y.Fechacreacion))
                .ForMember(x => x.FechaModificacion, x => x.MapFrom(y => y.Fechamodificacion))
                .ForMember(x => x.CreadoPor, x => x.MapFrom(y => y.Creadopor))
                .ForMember(x => x.ModificadoPor, x => x.MapFrom(y => y.Modificadopor));

            #region Mark

            _ = this.CreateMap<NozzleMarkViewModel, NozzleMarksDTO>().ReverseMap();

            #endregion

            #endregion

            #region DTO to DTO

            _ = this.CreateMap<InformationArtifactDTO, AllChangingTablesArtifactDTO>()
                .ForPath(x => x.Changetables, x => x.MapFrom(y => y.ChangingTablesArtifact))
                .ForPath(x => x.Tapbaan, x => x.MapFrom(y => y.TapBaan));

            _ = this.CreateMap<InformationArtifactDTO, AllCharacteristicsArtifactDTO>()
                .ForPath(x => x.ListEnfriamientos, x => x.MapFrom(y => y.CharacteristicsArtifact))
                .ForPath(x => x.CodOrden, x => x.MapFrom(y => y.GeneralArtifact.OrderCode))
                .ForPath(x => x.Connections, x => x.MapFrom(y => y.Connections))
                .ForPath(x => x.Derivations, x => x.MapFrom(y => y.Derivations))
                .ForPath(x => x.NBAI, x => x.MapFrom(y => y.NBAI))
                .ForPath(x => x.NBAINeutro, x => x.MapFrom(y => y.NBAINeutro))
                .ForPath(x => x.Taps, x => x.MapFrom(y => y.Taps))
                .ForPath(x => x.VoltageKV, x => x.MapFrom(y => y.VoltageKV));
            #endregion

        }
    }
}
