using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api.Dtos.Nozzle
{
    public class RecordNozzleInformationDto
    {
        public string NoSerie { get; set; }
        public string NoSerieBoq { get; set; }
        public decimal Orden { get; set; }
        public string Posicion { get; set; }
        public decimal IdMarca { get; set; }
        public string Marca { get; set; }
        public decimal IdTipo { get; set; }
        public string Tipo { get; set; }
        public decimal FactorPotencia { get; set; }
        public decimal Capacitancia { get; set; }
        public decimal Corriente { get; set; }
        public decimal Voltaje { get; set; }
        public bool Prueba { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
