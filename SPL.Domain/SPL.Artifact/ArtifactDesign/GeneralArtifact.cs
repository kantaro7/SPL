using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Artifact.ArtifactDesign
{
    public class GeneralArtifact
    {
        public GeneralArtifact()
        {
        }
        public GeneralArtifact(string orderCode, string descripcion, decimal? phases, string customerName, decimal? frecuency, string poNumeric, decimal? altitudeF1, string altitudeF2, decimal? typetrafoid, string tipoUnidad, decimal? applicationid, decimal? standardid, string norma, decimal? languageId, string claveIdioma, decimal? polarityId, string polarityOther, string desplazamientoAngular, string pTipoAceite, string pMarcaAceite, string creadopor, DateTime fechacreacion, string modificadopor, DateTime? fechamodificacion)
        {
            OrderCode = orderCode;
            Descripcion = descripcion;
            Phases = phases;
            CustomerName = customerName;
            Frecuency = frecuency;
            PoNumeric = poNumeric;
            AltitudeF1 = altitudeF1;
            AltitudeF2 = altitudeF2;
            Typetrafoid = typetrafoid;
            TipoUnidad = tipoUnidad;
            Applicationid = applicationid;
            Standardid = standardid;
            Norma = norma;
            LanguageId = languageId;
            ClaveIdioma = claveIdioma;
            PolarityId = polarityId;
            PolarityOther = polarityOther;
            DesplazamientoAngular = desplazamientoAngular;
            Creadopor = creadopor;
            Fechacreacion = fechacreacion;
            Modificadopor = modificadopor;
            Fechamodificacion = fechamodificacion;
            TipoAceite = pTipoAceite;
            MarcaAceite = pMarcaAceite;

        }
        #region Properties
        public string OrderCode { get; set; }
        public string Descripcion { get; set; }
        public decimal? Phases { get; set; }
        public string CustomerName { get; set; }
        public decimal? Frecuency { get; set; }
        public string PoNumeric { get; set; }
        public decimal? AltitudeF1 { get; set; }
        public string AltitudeF2 { get; set; }
        public decimal? Typetrafoid { get; set; }
        public string TipoUnidad { get; set; }
        public decimal? Applicationid { get; set; }
        public decimal? Standardid { get; set; }
        public string Norma { get; set; }
        public decimal? LanguageId { get; set; }
        public string ClaveIdioma { get; set; }
        public decimal? PolarityId { get; set; }
        public string PolarityOther { get; set; }
        public string DesplazamientoAngular { get; set; }
        public string TipoAceite { get; set; }
        public string MarcaAceite { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }

        #endregion

        }

       
    }

