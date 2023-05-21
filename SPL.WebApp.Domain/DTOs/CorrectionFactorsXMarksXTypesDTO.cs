namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class CorrectionFactorsXMarksXTypesDTO
    {
        public decimal IdMarca { get; set; }
        public string Marca { get; set; }
        public decimal IdTipo { get; set; }
        public string Tipo { get; set; }
        public decimal Temperatura { get; set; }
        public decimal FactorCorr { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
        public int OperationType { get; set; }
    }
}
