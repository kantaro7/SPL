using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.WebApp.Domain.DTOs
{
    public class InformationOctavesDTO
    {
        public string NoSerie { get; set; }

        public string TipoInfo { get; set; }

        public string Altura { get; set; }



        public DateTime FechaDatos { get; set; }

        public string Hora { get; set; }



        public decimal D16 { get; set; }

        public decimal D20 { get; set; }

        public decimal D25 { get; set; }

        public decimal D315 { get; set; } //campo D31.5

        public decimal D40 { get; set; }

        public decimal D50 { get; set; }

        public decimal D63 { get; set; }

        public decimal D80 { get; set; }


        public decimal D100 { get; set; }


        public decimal D125 { get; set; }


        public decimal D160 { get; set; }

        public decimal D200 { get; set; }

        public decimal D250 { get; set; }

        public decimal D3151 { get; set; } // campo D315

        public decimal D400 { get; set; }
        public decimal D500 { get; set; }

        public decimal D630 { get; set; }

        public decimal D800 { get; set; }

        public decimal D1000 { get; set; }

        public decimal D1250 { get; set; }

        public decimal D1600 { get; set; }

        public decimal D2000 { get; set; }

        public decimal D2500 { get; set; }

        public decimal D3150 { get; set; }

        public decimal D4000 { get; set; }

        public decimal D5000 { get; set; }

        public decimal D6300 { get; set; }

        public decimal D8000 { get; set; }


        public decimal D10000 { get; set; }


        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
