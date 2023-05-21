using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Artifact.Infrastructure.Entities
{
    public partial class SplTipoUnidad
    {
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
