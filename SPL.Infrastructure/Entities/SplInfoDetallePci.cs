using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Artifact.Infrastructure.Entities
{
    public partial class SplInfoDetallePci
    {
        #region Properties

        public decimal IdRep { get; set; }

        public decimal Seccion { get; set; }

        public decimal Renglon { get; set; }

        public string PosPri { get; set; }

        public string PosSec { get; set; }

        public decimal Corriente { get; set; }

        public decimal CorrienteIrms { get; set; }

        public decimal Tension { get; set; }

        public decimal TensionRms { get; set; }

        public decimal Potencia { get; set; }

        public decimal PotenciaKw { get; set; }

        public decimal ResisPri { get; set; }

        public decimal ResisSec { get; set; }

        public decimal TensionPri { get; set; }

        public decimal TensionSec { get; set; }

        public decimal VnomPri { get; set; }

        public decimal VnomSec { get; set; }

        public decimal InomPri { get; set; }

        public decimal InomSec { get; set; }

        public decimal I2rPri { get; set; }

        public decimal I2rSec { get; set; }

        public decimal I2rTot { get; set; }

        public decimal? I2rTotCorr { get; set; }

        public decimal WcuCorr { get; set; }

        public decimal Wind { get; set; }

        public decimal WindCorr { get; set; }

        public decimal Wcu { get; set; }

        public decimal PorcR { get; set; }

        public decimal PorcZ { get; set; }

        public decimal PorcX { get; set; }

        public decimal Wfe20 { get; set; }

        public decimal PerdTotal { get; set; }

        public decimal PerdCorregidas { get; set; }

        #endregion
    }
}
