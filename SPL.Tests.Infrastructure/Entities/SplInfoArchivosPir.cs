using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Tests.Infrastructure.Entities
{
    public partial class SplInfoArchivosPir
    {
        public decimal IdRep { get; set; }
        public decimal Orden { get; set; }
        public byte[] Archivo { get; set; }
        public string Nombre { get; set; }
    }
}
