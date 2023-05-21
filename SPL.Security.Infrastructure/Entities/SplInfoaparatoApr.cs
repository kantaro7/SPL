using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Security.Infrastructure.Entities
{
    public partial class SplInfoaparatoApr
    {
        public string OrderCode { get; set; }
        public decimal ColumnTypeId { get; set; }
        public string ColumnTitle { get; set; }
        public decimal? OrderIndex { get; set; }
        public decimal? Qty { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
