using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Tests.Infrastructure.Entities
{
    public partial class SplInfoDetalleIsz
    {
        public decimal IdRep { get; set; }
        public decimal Renglon { get; set; }
        public string Posicion1 { get; set; }
        public string Posicion2 { get; set; }
        public decimal Tension1 { get; set; }
        public decimal Tension2 { get; set; }
        public decimal TensionVrms { get; set; }
        public decimal CorrienteIrms { get; set; }
        public decimal PotenciaKw { get; set; }
        public decimal PotenciaCorrKw { get; set; }
        public decimal PorcRo { get; set; }
        public decimal PorcJxo { get; set; }
        public decimal PorcZo { get; set; }
        public decimal ZBase { get; set; }
        public decimal ZOhms { get; set; }
    }
}
