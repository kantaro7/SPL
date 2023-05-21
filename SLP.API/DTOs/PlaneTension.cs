using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPL.Artifact.Api.DTOs
{
    public class PlaneTension
    {
        public string Unidad { get; set; }
        public string TipoTension { get; set; }
        public decimal Orden { get; set; }
        public string Posicion { get; set; }
        public decimal Tension { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
