using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Artifact.Infrastructure.Entities
{
    public partial class SplPlantillaBase
    {
        public string TipoReporte { get; set; }
        public string ClavePrueba { get; set; }
        public string ClaveIdioma { get; set; }
        public byte[] Plantilla { get; set; }
        public decimal ColumnasConfigurables { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
