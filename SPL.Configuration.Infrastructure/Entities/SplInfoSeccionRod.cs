using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Configuration.Infrastructure.Entities
{
    public partial class SplInfoSeccionRod
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public DateTime FechaPrueba { get; set; }
        public decimal Temperatura { get; set; }
        public string UmTemp { get; set; }
        public string TitTerm1 { get; set; }
        public string TitTerm2 { get; set; }
        public string TitTerm3 { get; set; }
        public decimal TempSe { get; set; }
        public string UmTempse { get; set; }
        public decimal Fc20 { get; set; }
        public decimal FcSe { get; set; }
        public decimal VaDesv { get; set; }
        public decimal VaMaxDis { get; set; }
        public decimal VaMinDis { get; set; }
        public decimal MaxDesv { get; set; }
        public decimal MaxDdis { get; set; }
        public decimal MinDdis { get; set; }
    }
}
