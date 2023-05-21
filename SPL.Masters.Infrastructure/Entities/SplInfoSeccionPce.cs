using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Masters.Infrastructure.Entities
{
    public partial class SplInfoSeccionPce
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public DateTime FechaPrueba { get; set; }
        public decimal VoltajeBase { get; set; }
        public string UmVolbase { get; set; }
        public decimal CapMin { get; set; }
        public string UmCapmin { get; set; }
        public decimal Frecuencia { get; set; }
        public string UmFrec { get; set; }
        public string PosAt { get; set; }
        public string PosBt { get; set; }
        public string PosTer { get; set; }
        public decimal Temperatura { get; set; }
        public string UmTemp { get; set; }
        public decimal VnInicio { get; set; }
        public decimal VnFin { get; set; }
        public decimal VnIntervalo { get; set; }
    }
}
