namespace SPL.Domain.SPL.Reports.ETD
{
    using System;
    using System.Collections.Generic;

    public class ETDTestsGeneral
    {
        public decimal IdReg { get; set; }
        public decimal IdCorte { get; set; }
        public bool BtDifCap { get; set; }
        public decimal? CapacidadBt { get; set; }
        public string Terciario2b { get; set; }
        public bool TerCapRed { get; set; }
        public decimal? CapacidadTer { get; set; }
        public string DevanadoSplit { get; set; }
        public DateTime UltimaHora { get; set; }
        public decimal FactorKw { get; set; }
        public decimal FactorAltitud { get; set; }
        public bool TipoRegresion { get; set; }
        public string Comentario { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
        public decimal? TorLim { get; set; }
        public List<decimal?> AwrLim { get; set; }
        public List<decimal?> HsrLim { get; set; }
        public List<decimal?> GradienteLim { get; set; }
        public List<ETDTests> ETDTests { get; set; }
    }
}
