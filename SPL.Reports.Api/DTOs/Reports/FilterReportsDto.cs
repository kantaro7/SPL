namespace SPL.Reports.Api.DTOs.Reports
{
    using System;

    public class FilterReportsDto
    {
        public string TipoReporte { get; set; }
        public decimal Posicion { get; set; }
        public string Catalogo { get; set; }
        public string TablaBd { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
