namespace SPL.Reports.Api.DTOs.Reports.ROD
{
    using System.ComponentModel.DataAnnotations;

    public class RODTestsDetailsDto
    {

        [DataType(DataType.Text)]
        [MaxLength(5, ErrorMessage = "El campo Posición solo puede tener {0} caracteres")]
        public string Position { get; set; }

        [Range(0.0001, 999.9999, ErrorMessage = "El campo Terminal 1 debe ser numérico mayor a cero considerando hasta 3 enteros y 4 decimales")]
        public decimal TerminalH1 { get; set; }

        [Range(0.0001, 999.9999, ErrorMessage = "El campo Terminal 2 debe ser numérico mayor a cero considerando hasta 3 enteros y 4 decimales")]
        public decimal TerminalH2 { get; set; }

        [Range(0.0001, 999.9999, ErrorMessage = "El campo Terminal 3 debe ser numérico mayor a cero considerando hasta 3 enteros y 4 decimales")]
        public decimal TerminalH3 { get; set; }

        [Range(0.0001, 999.9999, ErrorMessage = "El campo Resistencia Promedio debe ser numérico mayor a cero considerando hasta 3 enteros y 4 decimales")]
        public decimal AverageResistance { get; set; }

        [Range(0.0001, 999.9999, ErrorMessage = "El campo Correccion 20 debe ser numérico mayor a cero considerando hasta 3 enteros y 4 decimales")]
        public decimal PercentageA { get; set; }

        [Range(0.0001, 999.9999, ErrorMessage = "El campo Correccion SE debe ser numérico mayor a cero considerando hasta 3 enteros y 4 decimales")]
        public decimal PercentageB { get; set; }

        public decimal Desv { get; set; }

        public decimal DesvDesign { get; set; }

        public ResistDesignDto ResistDesigns { get; set; }
    }
}
