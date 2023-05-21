using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Masters.Infrastructure.Entities
{
    public partial class SplInfoaparatoLab
    {
        public string OrderCode { get; set; }
        public string TextTestRoutine { get; set; }
        public string TextTestDielectric { get; set; }
        public string TextTestPrototype { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
