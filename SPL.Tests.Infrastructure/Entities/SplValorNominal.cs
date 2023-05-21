using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Tests.Infrastructure.Entities
{
    public partial class SplValorNominal
    {
        public string Unidad { get; set; }
        public decimal OperationId { get; set; }
        public decimal Orden { get; set; }
        public string Posicion { get; set; }
        public decimal Hvvolts { get; set; }
        public decimal Lvvolts { get; set; }
        public decimal ComboNumeric { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
