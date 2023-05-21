using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Reports.Infrastructure.Entities
{
    public partial class SplInfoGeneralEtd
    {
        public decimal IdRep { get; set; }
        public DateTime FechaRep { get; set; }
        public string NoSerie { get; set; }
        public decimal NoPrueba { get; set; }
        public string ClaveIdioma { get; set; }
        public string Cliente { get; set; }
        public string Capacidad { get; set; }
        public bool Resultado { get; set; }
        public string NombreArchivo { get; set; }
        public byte[] Archivo { get; set; }
        public string TipoReporte { get; set; }
        public string ClavePrueba { get; set; }
        public decimal IdCorte { get; set; }
        public decimal IdReg { get; set; }
        public int BtDifCap { get; set; }
        public decimal? CapacidadBt { get; set; }
        public string TerBt2 { get; set; }
        public int TerCapRed { get; set; }
        public decimal? CapacidadTer { get; set; }
        public string DevanadoSplit { get; set; }
        public decimal FactorKw { get; set; }
        public decimal FactorAlt { get; set; }
        public int TipoRegresion { get; set; }
        public int CorrXKw { get; set; }
        public string Comentario { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
