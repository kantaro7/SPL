using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Configuration.Api.DTOs.Configuration
{
    public class HeaderCuttingDataDto
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

        public List<SectionCuttingDataDto> SectionCuttingData { get; set; }
    }
}
