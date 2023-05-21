namespace SPL.Domain.SPL.Reports.ETD
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ETDConfigFileReport
    {
        public string Proceso { get; set; }
        public string Hoja { get; set; }
        public decimal Orden { get; set; }
        public string ClaveIdioma { get; set; }
        public string Etiqueta { get; set; }
        public decimal? Seccion { get; set; }
        public decimal? Consecutivo { get; set; }
        public string IniEtiqueta { get; set; }
        public string FinEtiqueta { get; set; }
        public string IniDato { get; set; }
        public string FinDato { get; set; }
        public string TipoDato { get; set; }
        public string Formato { get; set; }
        public decimal? CantDatos { get; set; }
        public string Validacion { get; set; }
        public string Campo1 { get; set; }
        public string Tabla1 { get; set; }
        public string Campo2 { get; set; }
        public string Tabla2 { get; set; }
    }
}
