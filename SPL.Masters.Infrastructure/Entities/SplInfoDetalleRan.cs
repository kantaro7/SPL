using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Masters.Infrastructure.Entities
{
    public partial class SplInfoDetalleRan
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public decimal Renglon { get; set; }
        public DateTime FechaPrueba { get; set; }
        public string Descripcion { get; set; }
        public decimal? Medicion { get; set; }
        public string UmMedicion { get; set; }
        public decimal Vcd { get; set; }
        public decimal? Limite { get; set; }
        public string Duracion { get; set; }
        public decimal Tiempo { get; set; }
        public string UmTiempo { get; set; }
        public string Valido { get; set; }
    }
}
