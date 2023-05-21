using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Masters.Infrastructure.Entities
{
    public partial class SplInfoaparatoBoqdet
    {
        public string NoSerie { get; set; }
        public string NoSerieBoq { get; set; }
        public decimal Orden { get; set; }
        public string Posicion { get; set; }
        public decimal IdMarca { get; set; }
        public decimal IdTipo { get; set; }
        public decimal FactorPotencia { get; set; }
        public decimal Capacitancia { get; set; }
        public decimal Corriente { get; set; }
        public decimal Voltaje { get; set; }
        public bool Prueba { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
        public decimal FactorPotencia2 { get; set; }
        public decimal Capacitancia2 { get; set; }
    }
}
