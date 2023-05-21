using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Artifact.Infrastructure.Entities
{
    public partial class SplInfoLaboratorio
    {
        public string Laboratorio { get; set; }
        public decimal AreaPiso { get; set; }
        public decimal AreaTecho { get; set; }
        public decimal AreaPared1 { get; set; }
        public decimal AreaPared2 { get; set; }
        public decimal AreaPuerta1 { get; set; }
        public decimal AreaPuerta2 { get; set; }
        public decimal Sv { get; set; }
        public decimal Alfa { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
