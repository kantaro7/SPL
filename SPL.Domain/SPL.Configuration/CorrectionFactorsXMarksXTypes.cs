using System;
using System.ComponentModel.DataAnnotations;

namespace SPL.Domain.SPL.Configuration
{
    public class CorrectionFactorsXMarksXTypes
    {
        public decimal IdMarca { get; set; }
        public string Marca { get; set; }
        public decimal IdTipo { get; set; }
        public string Tipo { get; set; }
        public decimal Temperatura { get; set; }

        [Range(1, 99999.99, ErrorMessage = "El campo Factor de Correccion debe ser numérico mayor a cero considerando hasta 5 enteros y 2 decimales")]
        public decimal FactorCorr { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
