using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api.Dtos.ArtifactDesing
{
    public class GeneralArtifactDto
    {

        public GeneralArtifactDto()
        {
           
        }
        public GeneralArtifactDto(string orderCode, string descripcion, decimal? phases, string customerName, decimal? frecuency, string poNumeric, decimal? altitudeF1, string altitudeF2, decimal? typetrafoid, string tipoUnidad, decimal? applicationid, decimal? standardid, string norma, decimal? languageId, string claveIdioma, decimal? polarityId, string polarityOther, string desplazamientoAngular, string pMarcaAceite, string pTipoAceite, string creadopor, DateTime fechacreacion, string modificadopor, DateTime? fechamodificacion)
        {
            OrderCode = orderCode;
            Descripcion = descripcion;
            Phases = phases;
            CustomerName = customerName;
            Frecuency = frecuency;
            PoNumeric = poNumeric;
            AltitudeF1 = altitudeF1;
            AltitudeF2 = altitudeF2;
            TypeTrafoId = typetrafoid;
            TipoUnidad = tipoUnidad;
            Applicationid = applicationid;
            StandardId = standardid;
            Norma = norma;
            LanguageId = languageId;
            ClaveIdioma = claveIdioma;
            PolarityId = polarityId;
            PolarityOther = polarityOther;
            DesplazamientoAngular = desplazamientoAngular;
            CreadoPor = creadopor;
            FechaCreacion = fechacreacion;
            ModificadoPor = modificadopor;
            FechaModificacion = fechamodificacion;
            OilBrand = pMarcaAceite;
            OilType = pTipoAceite;
        }
        #region Properties
        //[Required(ErrorMessage = "El número de serie es requerido")]
        public string OrderCode { get; set; }
        public string Descripcion { get; set; }
        public decimal? Phases { get; set; }
        //[Required(ErrorMessage = "El cliente es requerido")]
        //[MaxLength(200, ErrorMessage = "El cliente no puede superar los 200 caracteres")]
        public string CustomerName { get; set; }
        public decimal? Frecuency { get; set; }
        public string PoNumeric { get; set; }
        public decimal? AltitudeF1 { get; set; }
        public string AltitudeF2 { get; set; }
        public decimal? TypeTrafoId { get; set; }
        public string TipoUnidad { get; set; }
        public decimal? Applicationid { get; set; }
        public decimal? StandardId { get; set; }
        public string Norma { get; set; }
        public decimal? LanguageId { get; set; }
        public string ClaveIdioma { get; set; }
        public decimal? PolarityId { get; set; }
        public string PolarityOther { get; set; }
        public string OilType { get; set; }
        public string OilBrand { get; set; }
        public string DesplazamientoAngular { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }

        #endregion

        }
    }

