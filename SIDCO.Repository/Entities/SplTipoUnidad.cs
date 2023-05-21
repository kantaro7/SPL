using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class SplTipoUnidad
    {
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modifiadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
