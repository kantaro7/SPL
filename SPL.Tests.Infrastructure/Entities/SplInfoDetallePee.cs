using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Tests.Infrastructure.Entities
{
    public partial class SplInfoDetallePee
    {
        public decimal IdRep { get; set; }
        public decimal Renglon { get; set; }
        public string CoolingType { get; set; }
        public decimal TensionRms { get; set; }
        public decimal CorrienteRms { get; set; }
        public decimal PotenciaKw { get; set; }
        public decimal KwauxGar { get; set; }
        public decimal MvaauxGar { get; set; }
    }
}
