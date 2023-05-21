using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Reports.Infrastructure.Entities
{
    public partial class SplDatosgralEst
    {
        public decimal IdReg { get; set; }
        public string NoSerie { get; set; }
        public decimal Intervalo { get; set; }
        public string UmIntervalo { get; set; }
        public string PosAt { get; set; }
        public string PosBt { get; set; }
        public string PosTer { get; set; }
        public string Sobrecarga { get; set; }
        public int BtDifCap { get; set; }
        public decimal? CapacidadBt { get; set; }
        public string TerBt2 { get; set; }
        public int TerCapRed { get; set; }
        public decimal? CapacidadTer { get; set; }
        public string ClaveIdioma { get; set; }
        public string Conexion { get; set; }
        public string DevanadoSplit { get; set; }
        public decimal AltitudeF1 { get; set; }
        public string AltitudeF2 { get; set; }
        public decimal FactAlt { get; set; }
        public string MaterialDevanado { get; set; }
        public bool Estatus { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
