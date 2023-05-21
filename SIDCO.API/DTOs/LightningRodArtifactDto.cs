using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCO.API.DTOs
{
   public class LightningRodArtifactDto
    {
        public string OrderCode { get; set; }
        public decimal ColumnTypeId { get; set; }
        public string ColumnTitle { get; set; }
        public decimal? OrderIndex { get; set; }
        public decimal? Qty { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
