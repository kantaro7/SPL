using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports
{
    public class ConsolidatedReport
    {
        public long ID_REP { get; set; }
        public string TIPO_REPORTE { get; set; }
        public string NOMBRE_REPORTE { get; set; }
        public string ID_PRUEBA { get; set; }
        public string PRUEBA { get; set; }
        public string FILTROS { get; set; }
        public DateTime? FECHA { get; set; }
        public string COMENTARIOS { get; set; }
        public string AGRUPACION { get; set; }
        public string AGRUPACION_EN { get; set; }
        public string DESCRIPCION_EN { get; set; }
        public string IDIOMA { get; set; }
        public bool RESULTADO { get; set; }
    }
}
