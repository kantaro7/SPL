using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Reports.Infrastructure.Entities
{
    public partial class SplValidationTestsIsz
    {
        public string TipoUnidad { get; set; }
        public string ArregloDev { get; set; }
        public string MedicionDev { get; set; }
        public string Minimo { get; set; }
        public string Promedio { get; set; }
        public string Maximo { get; set; }
    }
}
