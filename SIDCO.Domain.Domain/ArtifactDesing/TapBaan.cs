using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCO.Domain.Artifactdesign
{
    public class TapBaan
    {
        public string OrderCode { get; set; }
        public decimal? ComboNumericSc { get; set; }
        public decimal? CantidadSupSc { get; set; }
        public decimal? PorcentajeSupSc { get; set; }
        public decimal? CantidadInfSc { get; set; }
        public decimal? PorcentajeInfSc { get; set; }
        public decimal? NominalSc { get; set; }
        public decimal? IdentificacionSc { get; set; }
        public decimal? InvertidoSc { get; set; }
        public decimal? ComboNumericBc { get; set; }
        public decimal? CantidadSupBc { get; set; }
        public decimal? PorcentajeSupBc { get; set; }
        public decimal? CantidadInfBc { get; set; }
        public decimal? PorcentajeInfBc { get; set; }
        public decimal? NominalBc { get; set; }
        public decimal? IdentificacionBc { get; set; }
        public decimal? InvertidoBc { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
