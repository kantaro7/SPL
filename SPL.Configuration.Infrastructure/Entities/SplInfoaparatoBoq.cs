using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Configuration.Infrastructure.Entities
{
    public partial class SplInfoaparatoBoq
    {
        public string OrderCode { get; set; }
        public decimal ColumnTypeId { get; set; }
        public string ColumnTitle { get; set; }
        public decimal? OrderIndex { get; set; }
        public decimal? Qty { get; set; }
        public decimal? VoltageClass { get; set; }
        public string VoltageClassOther { get; set; }
        public decimal? BilClass { get; set; }
        public string BilClassOther { get; set; }
        public decimal? CurrentAmps { get; set; }
        public decimal? CurrentAmpsReq { get; set; }
        public decimal? CorrienteUnidad { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
