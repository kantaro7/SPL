namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class ColumnTitleCEMReportsDTO
    {
        public decimal Tipo { get; set; }
        public string PosSec { get; set; }
        public string ClaveIdioma { get; set; }
        public string TituloPos { get; set; }
        public string Columna1 { get; set; }
        public string Columna2 { get; set; }
        public string Columna3 { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }

        public string PrimerRenglon { get; set; }
        public string SegundoRenglon { get; set; }
        public string TitPos1 { get; set; }
        public string TitPos2 { get; set; }
        public decimal PosColumna { get; set; }
    }
}
