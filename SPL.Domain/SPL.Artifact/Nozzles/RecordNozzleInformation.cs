using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Artifact.Nozzles
{
    public class RecordNozzleInformation
    {
        public string NoSerie { get; set; }
        public string NoSerieBoq { get; set; }
        public decimal Orden { get; set; }
        public string Posicion { get; set; }

        [Range(1, 99999, ErrorMessage = "El campo Id Marca debe ser numérico mayor a cero considerando hasta 5 enteros")]
        public decimal IdMarca { get; set; }
        public string Marca { get; set; }

        [Range(1, 99999, ErrorMessage = "El campo Id Tipo debe ser numérico mayor a cero considerando hasta 5 enteros")]
        public decimal IdTipo { get; set; }
        public string Tipo { get; set; }

        [Range(1, 999999.999, ErrorMessage = "El campo Factor Potencia debe ser numérico mayor a cero considerando hasta 6 enteros y 3 decimales")]
        public decimal FactorPotencia { get; set; }

        [Range(1, 999999, ErrorMessage = "El campo Capaci debe ser numérico mayor a cero considerando hasta 6 enteros")]
        public decimal Capacitancia { get; set; }
        public decimal FactorPotencia2{ get; set; }
        public decimal Capacitancia2{ get; set; }
        public decimal Corriente { get; set; }
        public decimal Voltaje { get; set; }
        public bool Prueba { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
