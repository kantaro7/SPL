using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Configuration.Api.DTOs.Configuration
{
    public class InformationLaboratoriesDto
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
