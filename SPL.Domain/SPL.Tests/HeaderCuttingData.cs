using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Tests
{
    public class HeaderCuttingData
    {
        public decimal IdCorte { get; set; }
        public string NoSerie { get; set; }
        public decimal IdReg { get; set; }
        public decimal KwPrueba { get; set; }
        public decimal Constante { get; set; }
        public DateTime UltimaHora { get; set; }
        public DateTime PrimerCorte { get; set; }
        public DateTime? SegundoCorte { get; set; }
        public DateTime? TercerCorte { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }

        //Me debes pasar el AOR es el valor de los datos de estabilización para la fecha y hora establecida en el campo “Ultima Hora”. el metodo es--> GetStabilizationData
        public decimal AOR { get; set; }
        //Me debes pasar el TOR es el valor de los datos de estabilización para la fecha y hora establecida en el campo “Ultima Hora”. el metodo es--> GetStabilizationData
        public decimal TOR { get; set; }
        //Me debes pasar el AmbProm es el valor de los datos de estabilización para la fecha y hora establecida en el campo “Ultima Hora”. el metodo es--> GetStabilizationData
        public decimal AmbProm { get; set; }



       
        public List<SectionCuttingData> SectionCuttingData { get; set; }
    }
}
