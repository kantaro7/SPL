using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Configuration.Infrastructure.Entities
{
    public partial class SplInfoSeccionFpb
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public DateTime FechaPrueba { get; set; }
        public decimal TensionPrueba { get; set; }
        public string UmTension { get; set; }
        public decimal Temperatura { get; set; }
        public string UmTemp { get; set; }
        public decimal TempPorcfp { get; set; }
        public decimal TempFptand { get; set; }
        public decimal AceptFp { get; set; }
        public decimal AceptCap { get; set; }
    }
}
