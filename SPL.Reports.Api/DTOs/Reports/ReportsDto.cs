namespace SPL.Reports.Api.DTOs.Reports
{
    using System;

    public class ReportsDto
    {
        public string TipoReporte { get; set; }
        public string Descripcion { get; set; }
        public string Creadopor { get; set; }
        public string AgrupacionEn { get; set; }
        public string Agrupacion { get; set; }
        public string DescripcionEn { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
