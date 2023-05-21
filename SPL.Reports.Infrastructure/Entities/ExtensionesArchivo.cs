using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Reports.Infrastructure.Entities
{
    public partial class ExtensionesArchivo
    {
        public ExtensionesArchivo()
        {
            PesoArchivos = new HashSet<PesoArchivo>();
        }

        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public long TipoArchivo { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<PesoArchivo> PesoArchivos { get; set; }
    }
}
