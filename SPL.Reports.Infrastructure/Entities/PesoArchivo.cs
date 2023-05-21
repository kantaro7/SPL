using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Reports.Infrastructure.Entities
{
    public partial class PesoArchivo
    {
        public long Id { get; set; }
        public long? ExtensionArchivo { get; set; }
        public string MaximoPeso { get; set; }
        public long IdModulo { get; set; }

        public virtual ExtensionesArchivo ExtensionArchivoNavigation { get; set; }
        public virtual SysModulo IdModuloNavigation { get; set; }
    }
}
