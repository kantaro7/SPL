using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Tests.Infrastructure.Entities
{
    public partial class SplFactorcorFpb
    {
        public decimal IdMarca { get; set; }
        public decimal IdTipo { get; set; }
        public decimal Temperatura { get; set; }
        public decimal FactorCorr { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
