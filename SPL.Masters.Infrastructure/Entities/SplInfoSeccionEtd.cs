﻿using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Masters.Infrastructure.Entities
{
    public partial class SplInfoSeccionEtd
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public DateTime FechaPrueba { get; set; }
        public decimal? NivelTension { get; set; }
        public string CoolingType { get; set; }
        public decimal OverElevation { get; set; }
        public string PosAt { get; set; }
        public string PosBt { get; set; }
        public string PosTer { get; set; }
        public decimal Capacidad { get; set; }
        public decimal AltitudeF1 { get; set; }
        public string AltitudeF2 { get; set; }
        public string Sobrecarga { get; set; }
        public decimal? Perdidas { get; set; }
        public decimal? Corriente { get; set; }
        public decimal? ResistCorte { get; set; }
        public decimal? TempResistCorte { get; set; }
        public decimal? FactorK { get; set; }
        public decimal? ResistTcero { get; set; }
        public decimal? TempDev { get; set; }
        public decimal? GradienteDev { get; set; }
        public decimal? ElevPromDev { get; set; }
        public decimal? ElevPtoMasCal { get; set; }
        public decimal? TempPromAceite { get; set; }
        public string Terminal { get; set; }
        public string UmResistencia { get; set; }
        public decimal? TorLimite { get; set; }
        public decimal? AwrLim { get; set; }
        public decimal? HsrLim { get; set; }
        public decimal? GradienteLim { get; set; }
        public bool? Resultado { get; set; }
    }
}
