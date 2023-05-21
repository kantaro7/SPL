using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Configuration.Infrastructure.Entities
{
    public partial class SplInfoSeccionCgd
    {
        public decimal IdRep { get; set; }
        public decimal Seccion { get; set; }
        public DateTime FechaPrueba { get; set; }
        public decimal? HrsTemperatura { get; set; }
        public string Metodo { get; set; }
        public string Resultado { get; set; }
        public string Notas { get; set; }
    }
}
