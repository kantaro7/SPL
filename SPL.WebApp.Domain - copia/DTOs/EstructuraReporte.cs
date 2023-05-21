using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.WebApp.Domain.DTOs
{
    public class EstructuraReporte
    {
        public string PrimeraColumna { get; set; }
        public string DevanadoEnergizado { get; set; }
        public string DevanadoAterrizado { get; set; }
        public int Columna { get; set; }
    }
}
