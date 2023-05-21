using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCO.Domain.Domain.Models
{
   public class ChangingTablesArtifact
    {
        public string OrderCode { get; set; }
        public decimal ColumnTypeId { get; set; }
        public string ColumnTitle { get; set; }
        public decimal? OrderIndex { get; set; }
        public decimal? OperationId { get; set; }
        public string FlagRcbnFcbn { get; set; }
        public decimal? DerivId { get; set; }
        public string DerivOther { get; set; }
        public decimal? DerivId2 { get; set; }
        public string Deriv2Other { get; set; }
        public decimal? Taps { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
