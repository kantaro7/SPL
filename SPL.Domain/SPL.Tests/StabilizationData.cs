namespace SPL.Domain.SPL.Tests
{
    using System;
    using System.Collections.Generic;

    public class StabilizationData
    {
        public decimal IdReg { get; set; }
        public string NoSerie { get; set; }
        public DateTime FechaDatos { get; set; }
        public string CoolingType { get; set; }
        public decimal OverElevation { get; set; }
        public decimal FactEnf { get; set; }
        public decimal Intervalo { get; set; }
        public string UmIntervalo { get; set; }
        public string PosAt { get; set; }
        public string PosBt { get; set; }
        public string PosTer { get; set; }
        public decimal Capacidad { get; set; }
        public string DevanadoSplit { get; set; }
        public decimal AltitudeF1 { get; set; }
        public string AltitudeF2 { get; set; }
        public decimal FactAlt { get; set; }
        public decimal PorcCarga { get; set; }
        public string Sobrecarga { get; set; }
        public decimal CantTermoPares { get; set; }
        public decimal? Perdidas { get; set; }
        public decimal? Corriente { get; set; }
        public bool Estatus { get; set; }
        public int CantEstables { get; set; }
        public int CantInestables { get; set; }
        public List<StabilizationDetailsData> StabilizationDataDetails { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
