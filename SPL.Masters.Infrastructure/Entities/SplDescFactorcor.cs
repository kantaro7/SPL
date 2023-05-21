﻿using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Masters.Infrastructure.Entities
{
    public partial class SplDescFactorcor
    {
        public string Especificacion { get; set; }
        public string ClaveIdioma { get; set; }
        public string Descripcion { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
