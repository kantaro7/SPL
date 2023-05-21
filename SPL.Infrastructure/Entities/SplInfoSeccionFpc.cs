using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Artifact.Infrastructure.Entities
{
    public partial class SplInfoSeccionFpc
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public DateTime FechaPrueba { get; set; }
        public decimal TensionPrueba { get; set; }
        public string UmTension { get; set; }
        public decimal TempAceiteSup { get; set; }
        public string UmTempacSup { get; set; }
        public decimal TempAceiteInf { get; set; }
        public string UmTempacInf { get; set; }
        public decimal? FactorCorr { get; set; }
        public decimal TempProm { get; set; }
        public decimal TempCt { get; set; }
        public decimal AceptCap { get; set; }
        public decimal AcepFp { get; set; }
    }
}
