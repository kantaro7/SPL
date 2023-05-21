using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Masters.Infrastructure.Entities
{
    public partial class SysModulo
    {
        public SysModulo()
        {
            PesoArchivos = new HashSet<PesoArchivo>();
        }

        public long Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<PesoArchivo> PesoArchivos { get; set; }
    }
}
