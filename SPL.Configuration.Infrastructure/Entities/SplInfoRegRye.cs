using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Configuration.Infrastructure.Entities
{
    public partial class SplInfoRegRye
    {
        public decimal IdRep { get; set; }
        public DateTime FechaPrueba { get; set; }
        public decimal Capacidad { get; set; }
        public decimal PorcZ { get; set; }
        public decimal PerdidaVacio { get; set; }
        public decimal PerdidaCarga { get; set; }
        public decimal PerdidaEnf { get; set; }
        public decimal PerdidaTotal { get; set; }
        public decimal PorcX { get; set; }
        public decimal PorcR { get; set; }
        public decimal XEntreR { get; set; }
        public decimal FactPot1 { get; set; }
        public decimal FactPot2 { get; set; }
        public decimal FactPot3 { get; set; }
        public decimal FactPot4 { get; set; }
        public decimal FactPot5 { get; set; }
        public decimal? FactPot6 { get; set; }
        public decimal? FactPot7 { get; set; }
        public decimal PorcReg1 { get; set; }
        public decimal PorcReg2 { get; set; }
        public decimal PorcReg3 { get; set; }
        public decimal PorcReg4 { get; set; }
        public decimal PorcReg5 { get; set; }
        public decimal PorcReg6 { get; set; }
        public decimal PorcReg7 { get; set; }
        public decimal ValorW { get; set; }
        public decimal ValorG { get; set; }
    }
}
