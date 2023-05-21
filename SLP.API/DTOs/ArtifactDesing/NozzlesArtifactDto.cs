using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Api.DTOs.Artifactdesign
{
   public class NozzlesArtifactDto
    {
        public NozzlesArtifactDto() { }
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
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
