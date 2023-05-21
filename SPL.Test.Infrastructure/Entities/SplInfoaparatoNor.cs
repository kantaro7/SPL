﻿using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Tests.Infrastructure.Entities
{
    public partial class SplInfoaparatoNor
    {
        public string OrderCode { get; set; }
        public decimal Secuencia { get; set; }
        public string Descripcion { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
