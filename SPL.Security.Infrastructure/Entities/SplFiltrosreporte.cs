using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Security.Infrastructure.Entities
{
    public partial class SplFiltrosreporte
    {
        public string TipoReporte { get; set; }
        public decimal Posicion { get; set; }
        public string Catalogo { get; set; }
        public string TablaBd { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
