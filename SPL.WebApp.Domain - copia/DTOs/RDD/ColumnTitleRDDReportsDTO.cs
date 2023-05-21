namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class ColumnTitleRDDReportsDTO
    {
        public string TipoUnidad { get; set; }
        public string ClaveIdioma { get; set; }
        public decimal Renglon { get; set; }
        public string Columna1 { get; set; }
        public string Columna2 { get; set; }
        public string Columna3 { get; set; }
        public string Columna4 { get; set; }
        public string Columna5 { get; set; }
        public decimal ConstanteX { get; set; }
        public decimal VcPorcFp { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
