namespace SPL.WebApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Utils;
    public class ArtifactRecordViewModel
    {
        private readonly string ErrorMessage = "Solo se permiten números";
        public ArtifactRecordViewModel()
        {
            this.CharacteristicsArtifacts = new List<CharacteristicsArtifactViewModel>();
            this.NozzlesArtifacts = new List<NozzlesArtifactViewModel>();
            this.LightningRodArtifacts = new List<LightningRodArtifactViewModel>();
            this.ChangingTablesArtifacs = new List<ChangingTablesArtifacViewModel>();
            this.LabTestsArtifact = new LabTestsArtifactViewModel();
            this.RulesArtifacts = new List<RulesArtifactViewModel>();
        }

        #region General Properties

        [Required(ErrorMessage = "No. Serie es requerido")]
        [DisplayName("No. Serie")]
        public string NoSerie { get; set; }

        [Required(ErrorMessage = "OrderCodeArtifactRecord")]
        public string OrderCode { get; set; }

        public string Creadopor { get; set; }

        public DateTime Fechacreacion { get; set; }

        public string Modificadopor { get; set; }

        public DateTime? Fechamodificacion { get; set; }

        public SelectList OperationsItems { get; set; }

        public bool IsFromSPL { get; set; }

        public ArtifactUpdate Update { get; set; }

        #endregion

        #region Properties Tab General

        [MaxLength(100)]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [DataType(DataType.Text)]
        [DisplayName("Fases")]
        public string Phases { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(200)]
        [DisplayName("Cliente")]
        public string CustomerName { get; set; }

        [DisplayName("Frecuencia")]
        [ValidDecimal(ErrorMessage = "Solo se permiten números")]
        public decimal? Frecuency { get; set; }

        [MaxLength(50)]
        [DisplayName("P.O. No.")]
        public string PoNumeric { get; set; }

        [DisplayName("Altitud")]
        public string AltitudeF1 { get; set; }

        [MaxLength(200)]

        [DisplayName("AltitudeF2")]
        public string AltitudeF2 { get; set; }

        [DisplayName("Tipo")]
        public string Typetrafoid { get; set; }

        [DisplayName("Tipo Unidad")]
        public string TipoUnidad { get; set; }

        [DisplayName("Aplicación")]
        public string ApplicationId { get; set; }

        [DisplayName("Norma Aplicable")]
        public string StandardId { get; set; }

        [DisplayName("Norma Aplicable")]
        public string Norma { get; set; }
        public string NormaId { get; set; }

        [DisplayName("Norma Equivalente")]
        public string NormaEquivalente { get; set; }

        [DisplayName("Lenguaje")]
        public string LanguageId { get; set; }

        [DisplayName("Lenguaje Equivalente")]
        public string ClaveIdioma { get; set; }

        [DisplayName("Desfasamiento Angular/Polaridad")]
        public string PolarityId { get; set; }

        [DisplayName("Otra")]
        [MaxLength(50)]
        public string PolarityOther { get; set; }

        [DisplayName("Desplazamiento Angular Equiv")]
        public string DesplazamientoAngular { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [DisplayName("Tipo Aceite")]
        public string OilType { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [MaxLength(50)]
        [DisplayName("Marca Aceite")]
        public string OilBrand { get; set; }

        #endregion

        #region Properties Tab Characteristics

        public List<CharacteristicsArtifactViewModel> CharacteristicsArtifacts { get; set; }

        public string TensionKvAltaTension1 { get; set; }

        public string TensionKvAltaTension2 { get; set; }

        public string TensionKvAltaTension3 { get; set; }

        public string TensionKvAltaTension4 { get; set; }

        public string TensionKvBajaTension1 { get; set; }

        public string TensionKvBajaTension2 { get; set; }

        public string TensionKvBajaTension3 { get; set; }

        public string TensionKvBajaTension4 { get; set; }

        public string TensionKvSegundaBaja1 { get; set; }

        public string TensionKvSegundaBaja2 { get; set; }

        public string TensionKvSegundaBaja3 { get; set; }

        public string TensionKvSegundaBaja4 { get; set; }

        public string TensionKvTerciario1 { get; set; }

        public string TensionKvTerciario2 { get; set; }

        public string TensionKvTerciario3 { get; set; }

        public string TensionKvTerciario4 { get; set; }

        public string NbaiAltaTension { get; set; }

        public string NbaiBajaTension { get; set; }

        public string NbaiSegundaBaja { get; set; }

        public string NabaiTercera { get; set; }

        public string IdConexionAltaTension { get; set; }

        public string ConexionAltaTension { get; set; }

        public string OtraConexionAltaTension { get; set; }

        public string IdConexionBajaTension { get; set; }

        public string ConexionBajaTension { get; set; }

        public string OtraConexionBajaTension { get; set; }

        public string IdConexionSegundaBaja { get; set; }

        public string ConexionSegundaBaja { get; set; }

        public string OtraConexionSegundaBaja { get; set; }

        public string IdConexionTercera { get; set; }

        public string ConexionTercera { get; set; }

        public string OtraConexionTercera { get; set; }

        public string TipoDerivacionAltaTension { get; set; }

        public string ValorDerivacionUpAltaTension { get; set; }

        public string ValorDerivacionDownAltaTension { get; set; }

        public string TipoDerivacionAltaTension_2 { get; set; }

        public string ValorDerivacionUpAltaTension_2 { get; set; }

        public string ValorDerivacionDownAltaTension_2 { get; set; }

        public string TipoDerivacionBajaTension { get; set; }

        public string ValorDerivacionUpBajaTension { get; set; }

        public string ValorDerivacionDownBajaTension { get; set; }

        public string TipoDerivacionSegundaTension { get; set; }

        public string ValorDerivacionUpSegundaTension { get; set; }

        public string ValorDerivacionDownSegundaTension { get; set; }

        public string TipoDerivacionTerceraTension { get; set; }

        public string ValorDerivacionUpTerceraTension { get; set; }

        public string ValorDerivacionDownTerceraTension { get; set; }

        public string ConexionEquivalente { get; set; }

        public string ConexionEquivalente_2 { get; set; }

        public string ConexionEquivalente_3 { get; set; }

        public string ConexionEquivalente_4 { get; set; }

        public string IdConexionEquivalente { get; set; }

        public string IdConexionEquivalente2 { get; set; }

        public string IdConexionEquivalente3 { get; set; }

        public string IdConexionEquivalente4 { get; set; }

        public string TapsAltaTension { get; set; }

        public string TapsAltaTension_2 { get; set; }

        public string TapsBajaTension { get; set; }

        public string TapsSegundaBaja { get; set; }

        public string TapsTerciario { get; set; }

        public string ValorNbaiNeutroAltaTension { get; set; }

        public string ValorNbaiNeutroBajaTension { get; set; }

        public string ValorNbaiNeutroSegundaBaja { get; set; }

        public string ValorNbaiNeutroTercera { get; set; }
        #endregion

        #region Properties Tab Warranties

        [DisplayName("Lexc @ 100/110% Vn(%)")]
        public string Iexc100 { get; set; }

        public string Iexc110 { get; set; }

        [DisplayName("Pérd. Núcleo @ 100/110% Vn")]
        public string Kwfe100 { get; set; }

        public string Kwfe110 { get; set; }

        public string TolerancyKwfe { get; set; }

        public string KwcuMva { get; set; }

        public string KwcuKv { get; set; }

        [DisplayName("Pérd. Cu @")]
        public string Kwcu { get; set; }

        public string TolerancyKwCu { get; set; }

        public string Kwaux3 { get; set; }

        public string Kwaux4 { get; set; }

        public string Kwaux1 { get; set; }

        [DisplayName("Pérd. Aux @")]
        public string Kwaux2 { get; set; }

        public string TolerancyKwAux { get; set; }

        [DisplayName("Pérd. Totales @ 100/110% Vn")]
        public string Kwtot100 { get; set; }

        public string Kwtot110 { get; set; }

        public string TolerancyKwtot { get; set; }

        public string ZPositiveMva { get; set; }

        [DisplayName("Z+(H-X) / (H-Y) / (X-Y) (%) @")]
        public string ZPositiveHx { get; set; }

        public string ZPositiveHy { get; set; }

        public string ZPositiveXy { get; set; }

        public string TolerancyZpositive { get; set; }

        public string TolerancyZpositive2 { get; set; }

        public string NoiseOa { get; set; }

        public string NoiseFa1 { get; set; }

        public string NoiseFa2 { get; set; }

        [DisplayName("Nivel de Ruido")]
        public string CoolingTypes
        {
            get
            {
                string value = string.Empty;
                for (int i = 0; i < this.CharacteristicsArtifacts.Count; i++)
                {
                    if (i + 1 < this.CharacteristicsArtifacts.Count)
                    {
                        value += this.CharacteristicsArtifacts[i].CoolingType + "/";
                    }
                    else
                    {
                        value += this.CharacteristicsArtifacts[i].CoolingType;
                    }
                }
                return value;
            }
        }
        #endregion

        #region Properties Tab Nozzles

        public List<NozzlesArtifactViewModel> NozzlesArtifacts { get; set; }

        #endregion

        #region Properties Tab LightningRod
        public List<LightningRodArtifactViewModel> LightningRodArtifacts { get; set; }
        #endregion

        #region Properties Tab Changes
        public List<ChangingTablesArtifacViewModel> ChangingTablesArtifacs { get; set; }

        //public TapBaanViewModel TapBaan { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public string ComboNumericSc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string CantidadSupSc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string PorcentajeSupSc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string CantidadInfSc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string PorcentajeInfSc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string NominalSc { get; set; }

        public string IdentificacionSc { get; set; }
        public bool InvertidoSc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string ComboNumericBc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string CantidadSupBc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string PorcentajeSupBc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string CantidadInfBc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string PorcentajeInfBc { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string NominalBc { get; set; }
        public string IdentificacionBc { get; set; }
        public bool InvertidoBc { get; set; }

        #endregion

        #region Properties Test Labs

        public LabTestsArtifactViewModel LabTestsArtifact { get; set; }

        #endregion

        #region Properties norms
        public List<RulesArtifactViewModel> RulesArtifacts { get; set; }

        #endregion

        public string Errores { get; set; }

    }
}
