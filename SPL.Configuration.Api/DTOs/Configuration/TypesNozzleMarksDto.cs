using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Configuration.Api.DTOs.Configuration
{
    public class TypesNozzleMarksDto
    {
        public decimal IdMarca { get; set; }
        public decimal IdTipo { get; set; }
        public string Descripcion { get; set; }
        public bool Estatus { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
